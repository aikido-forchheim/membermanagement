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

        protected override int RowIndexFromTrainingParticipationTrainingParticipation(TrainingsTeilnahme tn)
        {
            return tn.MitgliedID - 1;  // db ids start with 1, array indeices with 0
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

            int iStringCol = m_iNrOfColsOnLeftSide;
            for (int iCol = 0; iCol < m_iNrOfCols - 1; ++iCol)
            {
                if (m_iColSum[iCol] > 0)
                {
                    Kurs kurs = m_db.KursFromId(iCol+1);
                    m_stringMatrix[0, iStringCol] = $"    { m_db.WeekDay(kurs.WochentagID).Substring(0, 2) } ";
                    m_stringMatrix[1, iStringCol] = $" {kurs.Zeit:hh}:{kurs.Zeit:mm} ";
                    ++iStringCol;
                }
            }
            m_stringMatrix[0, iStringCol] = " Lehrgänge";
            m_stringMatrix[1, iStringCol] = " Sondertr.";
            ++iStringCol;

            m_stringMatrix[0, iStringCol] = "";
            m_stringMatrix[1, iStringCol] = " Summe";
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
            m_iNrOfFooterRows = 2;

            Initialize
            ( 
                m_db.MaxMitgliedsNr() + 1,   // One additional row for pseudo member with Id = -1
                m_db.MaxKursNr() + 1         // One additional col for "Lehrgänge"
            );

            PrepareData();

            FillHeaderRows();
            FillMainRows();
            FillFooterRows();

            return m_stringMatrix;
        }
    }
}
