using System;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportBusinessLogic
{
    public class ReportMemberVsCourse : ReportBase
    {
        private DataGridView m_dataGridView = new DataGridView();

        public ReportMemberVsCourse(DatabaseWrapper db, DateTime datStart, DateTime datEnd)
            : base(db, datStart, datEnd)
        {
            m_iNrOfColsOnLeftSide = 1;   // column for Mitglieder
            m_iNrOfColsOnRightSide = 1;  // column for row sum
            m_iNrOfHeaderRows = 3;       // one empty row

            Initialize
            (
                m_db.MaxMitgliedsNr() + 1,   // One additional row for pseudo member with Id = -1
                m_db.MaxKursNr() + 1         // One additional col for "Lehrgänge"
            );

            CollectData
            (
                tn => true,
                tn => tn.MitgliedID - 1,  // db ids start with 1, array indices with 0
                tn => 
                {
                    return m_db.KursIdFromTrainingId(tn.TrainingID);
//                    int idKurs = m_db.KursIdFromTrainingId(tn.TrainingID);
//                    return ((idKurs == 0) ? idKurs : m_iNrOfCols) - 1;
                }
            );

            Array.Sort(m_Rows);
        }

        protected override void FillHeaderRows()
        {
            m_dataGridView[0, 0].Value = "                       Mitglied  ";
            m_dataGridView[0, 1].Value = "                         Nummer  ";

            FillCourseHeaderRows(m_dataGridView);
        }

        protected override string FormatFirstColElement(int iRow)
        {
            return Utilities.FormatMitglied(m_db.MitgliedFromId(m_Rows[iRow].idRow));
        }

        protected override string FormatMatrixElement(int iValue)
        {
            return Utilities.FormatNumber( iValue );
        }

        protected override string FormatColSumElement(int iValue)
        {
            return Utilities.FormatNumber( iValue );
        }

        public DataGridView GetMatrix()
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
