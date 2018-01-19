using System;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.BusinessLogic
{
    public class MemberVsCourse : MatrixReport
    {
        public MemberVsCourse(DatabaseWrapper db, DateTime datStart, DateTime datEnd)
            : base(db, datStart, datEnd) { }

        protected override bool IsRelevant(TrainingsTeilnahme tn)
        {
            return true;
        }

        protected override int RowIndexFromTrainingParticipation(TrainingsTeilnahme tn)
        {
            return tn.MitgliedID - 1;  // db ids start with 1, array indices with 0
        }

        protected override int ColIndexFromTrainingParticipation(TrainingsTeilnahme tn)
        {
            int? idKurs = m_db.TrainingFromId(tn.TrainingID).KursID;
            return idKurs.HasValue ? (idKurs.Value - 1) : m_iNrOfCols-1;
        }

        protected override void FillHeaderRows()
        {
            m_stringMatrix[0, 0] = "                       Mitglied  ";
            m_stringMatrix[1, 0] = "                         Nummer  ";

            FillCourseHeaderRows();
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

        public string[,] GetMatrix()
        {
            m_iNrOfColsOnLeftSide = 1;   // column for Mitglieder
            m_iNrOfColsOnRightSide = 1;  // column for row sum
            m_iNrOfHeaderRows = 3;       // one empty row

            Initialize
            ( 
                m_db.MaxMitgliedsNr() + 1,   // One additional row for pseudo member with Id = -1
                m_db.MaxKursNr() + 1         // One additional col for "Lehrgänge"
            );

            CollectData();

            Array.Sort(m_Rows);

            FillHeaderRows();
            FillMainRows();
            FillFooterRow("                     Insgesamt  ");

            return m_stringMatrix;
        }
    }
}
