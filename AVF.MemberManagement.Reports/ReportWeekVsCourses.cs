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
                filter: tn => idMember == tn.MitgliedID
            );

            m_label1.Text = "Trainingsteilnahme " + Globals.GetMemberDescription(idMember);
            m_label2.Text = Globals.GetTimeRangeDescription(datStart, datEnd);
        }

        protected override string MouseHeaderCellEvent(int col, bool action)
            => MouseCourseHeaderCellEvent(col, action);

        protected override string MouseMainDataAreaCellEvent(int row, int col, bool action)
            => String.Empty;

        protected override string MouseKeyCellEvent(int row, bool action)
        {
            if (action)
            {
                return String.Empty;
            }
            else
            {
                return $"Klicken für Details zu Trainings in dieser Woche (Noch nicht implementiert)";
            }
        }
    }
}
