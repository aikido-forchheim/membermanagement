using System;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.BusinessLogic
{
    public class MemberVsTraining: MatrixReport
    {
        private int m_idKurs;

        public MemberVsTraining(DatabaseWrapper db, DateTime datStart, DateTime datEnd)
            : base(db, datStart, datEnd) { }

        protected override bool IsRelevant(TrainingsTeilnahme tn)
        {
            int? idKurs = m_db.TrainingFromId(tn.TrainingID).KursID;
            return idKurs.HasValue ? (idKurs.Value == m_idKurs) : false;
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
            m_stringMatrix[0, iCol] = "                          Monat ";
            m_stringMatrix[1, iCol] = "                            Tag ";
            foreach (var training in m_db.TrainingsInPeriod(m_idKurs, m_datStart, m_datEnd))
            {
                ++iCol;
                m_stringMatrix[0, iCol] = $" {training.Termin:MM}";
                m_stringMatrix[1, iCol] = $" {training.Termin:dd}";
            }
            ++iCol;
            m_stringMatrix[1, iCol] = "    Summe";
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

        public string[,] GetMatrix( int idKurs )
        {
            m_idKurs = idKurs;
            m_iNrOfColsOnLeftSide = 1;   // column for Mitglieder
            m_iNrOfColsOnRightSide = 1;  // column for row sum
            m_iNrOfHeaderRows = 2;

            Initialize
            (
                m_db.MaxMitgliedsNr() + 1,   // One additional row for pseudo member with Id = -1
                m_db.MaxTrainingNr () + 1
            );

            CollectData( );

            Array.Sort(m_Rows);

            FillHeaderRows();
            FillMainRows();
            FillFooterRow("                     Insgesamt  ");

            return m_stringMatrix;
        }
    }
}
