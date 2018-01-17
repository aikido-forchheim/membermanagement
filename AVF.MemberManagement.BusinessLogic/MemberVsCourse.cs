using System;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.BusinessLogic
{
    public class MemberVsCourse
    {
        private DatabaseWrapper m_db;

        private DateTime m_datStart;
        private DateTime m_datEnd;

        private struct Row
        {
            public int idMember;
            public int[] aiValues;
        };

        private int m_iNrOfCols;
        private int m_iNrOfRows;

        private string[][] m_ColumnLabels;
        private Row[] m_Matrix;

        private int[] m_RowSum;
        private int[] m_ColSum;
        private int m_SumSum;

        private string FormatNumber(int i)
            => (i > 0) ? $"{ i,7 }" : "       ";

        public string FormatMitglied(Mitglied mitglied)
            => $"{ mitglied.Nachname,-12 } { mitglied.Vorname,-12 } ({ mitglied.Id,3 }) ";

        public string FormatMitglied(int idMember)
            => FormatMitglied(m_db.MitgliedFromId(idMember));

        public MemberVsCourse(DatabaseWrapper db, DateTime datStart, DateTime datEnd)
        {
            m_db = db;
            m_datStart = datStart;
            m_datEnd = datEnd;

            int colKursNull = m_db.MaxKursNr();

            m_iNrOfRows = m_db.MaxMitgliedsNr() + 1; // One additional row for pseudo member with Id = -1
            m_iNrOfCols = m_db.MaxKursNr() + 1;      // One additional col for "Lehrgänge"

            m_ColumnLabels = new string[2][];
            m_ColumnLabels[0] = new string[m_iNrOfCols];
            m_ColumnLabels[1] = new string[m_iNrOfCols];
            for (int iCol = 0; iCol < m_iNrOfCols - 1; ++iCol)
            {
                Kurs kurs = m_db.KursFromId(iCol + 1);
                m_ColumnLabels[0][iCol] = $"    { db.WeekDay(kurs.WochentagID).Substring(0, 2) } ";
                m_ColumnLabels[1][iCol] = $" {kurs.Zeit:hh}:{kurs.Zeit:mm} ";
            }
            m_ColumnLabels[0][m_iNrOfCols - 1] = " Lehrgänge";
            m_ColumnLabels[1][m_iNrOfCols - 1] = " Sondertr.  Summe";

            m_Matrix = new Row[m_iNrOfRows];

            for (int iRow = 0; iRow < m_iNrOfRows; ++iRow)
            {
                m_Matrix[iRow].aiValues = new int[m_iNrOfCols];
                m_Matrix[iRow].idMember = iRow;
            }

            m_RowSum = new int[m_iNrOfRows];
            m_ColSum = new int[m_iNrOfCols];

            foreach (var trainingsTeilnahme in m_db.TrainingsTeilnahme(m_datStart, m_datEnd))
            {
                int? idKurs = m_db.TrainingFromId(trainingsTeilnahme.TrainingID).KursID;
                int iCol = idKurs.HasValue ? (idKurs.Value - 1) : colKursNull;
                int iRow = trainingsTeilnahme.MitgliedID;
                ++m_Matrix[iRow].aiValues[iCol];
                ++m_ColSum[iCol];
                ++m_RowSum[iRow];
                ++m_SumSum;
            }

            Array.Sort(m_RowSum, m_Matrix);
            Array.Reverse(m_Matrix);
            Array.Reverse(m_RowSum);
        }

        public string[,] GetMatrix()
        {
            string[,] matrix;

            int iNrOfMatrixRows = 3;  // 3 for header rows

            for (int iRow = 0; iRow < m_iNrOfRows; ++iRow)
                if (m_RowSum[iRow] > 0)
                    ++iNrOfMatrixRows;

            iNrOfMatrixRows += 2;  // + 2 for col sum

            int iNrOfMatrixCols = m_iNrOfCols;
            ++iNrOfMatrixCols;                  // + 1 for member
            ++iNrOfMatrixCols;                  // + 1 for row sum

            matrix = new string[iNrOfMatrixRows, iNrOfMatrixCols];

            matrix[0, 0] = "                       Mitglied  ";
            matrix[1, 0] = "                         Nummer  ";

            for (int iCol = 1; iCol < m_iNrOfCols - 1; ++iCol)
            {
                Kurs kurs = m_db.KursFromId(iCol);
                matrix[0, iCol] = $"    { m_db.WeekDay(kurs.WochentagID).Substring(0, 2) } ";
                matrix[1, iCol] = $" {kurs.Zeit:hh}:{kurs.Zeit:mm} ";
            }
            matrix[0, m_iNrOfCols - 1] = " Lehrgänge";
            matrix[1, m_iNrOfCols - 1] = " Sondertr.";

            matrix[0, m_iNrOfCols] = "";
            matrix[1, m_iNrOfCols] = " Summe";

            int iStringRow = 3;

            for (int iRow = 0; iRow < m_iNrOfRows; ++iRow)
            {
                if (m_RowSum[iRow] > 0)
                {
                    matrix[iStringRow, 0] = FormatMitglied(m_Matrix[iRow].idMember);
                    for (int iCol = 1; iCol < m_iNrOfCols; ++iCol)
                    {
                        matrix[iStringRow, iCol] = FormatNumber(m_Matrix[iRow].aiValues[iCol - 1]);
                    }
                    matrix[iStringRow, m_iNrOfCols] = "  " + FormatNumber(m_RowSum[iRow]);
                    ++iStringRow;
                }
            }

            ++iStringRow;
            matrix[iStringRow, 0] = "                     Insgesamt  ";
            for (int iCol = 1; iCol < m_iNrOfCols; ++iCol)
            {
                matrix[iStringRow, iCol] = FormatNumber(m_ColSum[iCol - 1]);
            }
            matrix[iStringRow, m_iNrOfCols] = "  " + FormatNumber(m_SumSum);

            return matrix;
        }

        public string[][] GetColumnLabels() => m_ColumnLabels;
    }

}
