using System;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportWeekVsCourses : ReportForm
    {
        public ReportWeekVsCourses(DateTime datStart, DateTime datEnd, int idMember)
        {
            CreateModel
            (
                datStart, datEnd,
                new HorizontalAxisCourses(),
                new VerticalAxisWeeks(),
                (tn => idMember == tn.MitgliedID)
            );

            m_label1.Text = "Trainingsteilnahme " + Globals.GetMemberDescription(idMember);
            m_label2.Text = Globals.GetTimeRangeDescription(datStart, datEnd);
        }

        protected override string MouseCellEvent(int row, int col, bool action)
        {
            ReportForm newForm = null;
            string helpText = String.Empty;


            if (RowIsHeader(row))
            {
                if (ColIsInMainArea(col))
                {
                    if (action)
                        newForm = new ReportMemberVsTrainings(m_datStart, m_datEnd, m_xAxis.GetDbId(col));
                    else
                        helpText = $"Klicken für Details zum Kurs\n" + Globals.GetCourseDescription(m_xAxis.GetDbId(col));
                }
                else // column header, key or summary columns
                {
                    string columnName = m_dataGridView.Columns[col].HeaderText;
                    helpText = $"Klicken um nach {columnName} zu sortieren";
                }
            }
            else  // data row
            {
                if (ColIsInMainArea(col))
                {
                }
                else // key or summary 
                {
                    if (action)
                        newForm = null;
                    else
                        helpText = $"Klicken für Details zu Trainings in dieser Woche (Noch nicht implementiert)";
                }
            }

            if (newForm != null)
                newForm.Show();

            return helpText;
        }
    }
}
