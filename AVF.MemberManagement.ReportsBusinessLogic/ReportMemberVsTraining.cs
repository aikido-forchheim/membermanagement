using System;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportBusinessLogic
{
    public class ReportMemberVsTraining: ReportBase
    {
        private DataGridView m_dataGridView = new DataGridView();

        private int m_idKurs;

        static bool isAlwaysRelevant(TrainingsTeilnahme tn)
           => true;

        static bool isNeverRelevant(TrainingsTeilnahme tn)
            => false;

        public ReportMemberVsTraining(DatabaseWrapper db, DateTime datStart, DateTime datEnd, int idKurs)
            : base(db, datStart, datEnd)
        {
            m_idKurs = idKurs;
            m_iNrOfColsOnLeftSide = 1;   // column for Mitglieder
            m_iNrOfColsOnRightSide = 1;  // column for row sum
            m_iNrOfHeaderRows = 2;

            Initialize
            (
                m_db.MaxMitgliedsNr() + 1,   // One additional row for pseudo member with Id = -1
                m_db.MaxTrainingNr() + 1
            );

            CollectData
            (
                tn => 
                {
                    int? idKursX = m_db.TrainingFromId(tn.TrainingID).KursID;
                    return idKursX.HasValue ? (idKursX.Value == m_idKurs) : false;
                }
            );

            Array.Sort(m_Rows);
        }

        protected override int RowIndexFromTrainingParticipation(TrainingsTeilnahme tn)
        {
            return tn.MitgliedID - 1;  // db ids start with 1, array indeices with 0
        }

        protected override int ColIndexFromTrainingParticipation(TrainingsTeilnahme tn)
        {
            return tn.TrainingID - 1; // db ids start with 1, array indeices with 0
        }

        protected override void FillHeaderRows( )
        {
            int iCol = 0;
            m_dataGridView[iCol, 0].Value = "                          Monat ";
            m_dataGridView[iCol, 1].Value = "                            Tag ";
            foreach (var training in m_db.TrainingsInPeriod(m_idKurs, m_datStart, m_datEnd))
            {
                ++iCol;
                m_dataGridView[iCol, 0].Value = $" {training.Termin:MM}";
                m_dataGridView[iCol, 1].Value = $" {training.Termin:dd}";
            }
            ++iCol;
            m_dataGridView[iCol, 1].Value = "    Summe";
        }

        protected override string FormatFirstColElement(int iRow)
        {
            return Utilities.FormatMitglied(m_db.MitgliedFromId(m_Rows[iRow].idRow));
        }

        protected override string FormatMatrixElement(int iValue)
        {
            return (iValue == 1) ? "  X" : "   ";
        }

        protected override string FormatColSumElement(int iValue)
        {
            return (iValue > 0) ? $"{ iValue,3 }" : "   ";
        }

        public DataGridView GetMatrix( )
        {
            m_dataGridView.RowCount = GetNrOfRows();  // one footer row
            m_dataGridView.ColumnCount = GetNrOfCols();

            FillHeaderRows();
            FillMainRows(m_dataGridView);
            FillFooterRow(m_dataGridView, "                     Insgesamt  ");

            return m_dataGridView;
        }
    }
}
