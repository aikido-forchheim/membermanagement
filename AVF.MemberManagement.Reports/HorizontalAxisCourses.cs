using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    class HorizontalAxisCourses : HorizontalAxis
    {
        public HorizontalAxisCourses(DatabaseWrapper db, TrainingParticipationMatrix tpMatrix)
            : base(db, tpMatrix)
        {
            m_activeColsOnly = false;
        }

        public override int GetNrOfSrcElements()
            => m_db.MaxKursNr() + 1;

        public override int GetIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => m_db.KursIdFromTrainingId(tn.TrainingID);

        public override void FillHeaderRows(DataGridView dgv, int iNrOfColsOnLeftSide)
        {
            dgv.Columns[0].HeaderText = "Kalender\nwoche";

            int iDgvCol = iNrOfColsOnLeftSide;

            dgv.Columns[iDgvCol].HeaderText = "Lehrg.\netc.";
            dgv.Columns[iDgvCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            iDgvCol++;
            m_tpMatrix.ForAllCols
            (
                action: iCol =>
                {
                    dgv.Columns[iDgvCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    if (iCol > 0)
                    {
                        Kurs kurs = m_db.KursFromId(iCol);
                        dgv.Columns[iDgvCol++].HeaderText = $"{ m_db.WeekDay(kurs.WochentagID).Substring(0, 2) }\n{kurs.Zeit:hh}:{kurs.Zeit:mm}";
                    }
                },
                activeColsOnly: m_activeColsOnly
            );
            dgv.Columns[dgv.ColumnCount - 1].HeaderText = "\nSumme";
            dgv.Columns[dgv.ColumnCount - 1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
    }
}
