using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    class HorizontalAxisCourses : HorizontalAxis
    {
        public HorizontalAxisCourses(DatabaseWrapper db, TrainingParticipationReport coreReport)
            : base(db, coreReport)
        {
            m_activeColumnsOnly = false;
        }

        public override int GetNrOfSrcColumns()
            => m_db.MaxKursNr() + 1;

        public override int GetRowIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => m_db.KursIdFromTrainingId(tn.TrainingID);

        public override void FillHeaderRows(DataGridView dgv, int iNrOfColsOnLeftSide)
        {
            dgv.Columns[0].HeaderText = "Kalender\nwoche";

            int iDgvCol = iNrOfColsOnLeftSide;

            dgv.Columns[iDgvCol].HeaderText = "Lehrg.\netc.";
            dgv.Columns[iDgvCol].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            iDgvCol++;
            m_coreReport.ForAllColumns
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
                activeColumnsOnly: m_activeColumnsOnly
            );
            dgv.Columns[dgv.ColumnCount - 1].HeaderText = "\nSumme";
            dgv.Columns[dgv.ColumnCount - 1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
    }
}
