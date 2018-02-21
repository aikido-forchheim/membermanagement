using System;
using System.Diagnostics;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsTrainings : ReportForm
    {
        public ReportMemberVsTrainings(DateTime datStart, DateTime datEnd, int idKurs)
        {
            CreateModel
            (
                datStart, datEnd,
                new HorizontalAxisTrainings(),
                new VerticalAxisMembers(),
                filter: tn => Globals.DatabaseWrapper.KursIdFromTrainingId(tn.TrainingID) == idKurs
            );

            m_label1.Text = Globals.GetCourseDescription(idKurs);
            m_label2.Text = Globals.GetTimeRangeDescription(datStart, datEnd);
        }

        protected override string FormatMatrixElement(int iValue) 
            => (iValue > 0) ? $"X" : " ";

        protected override string MouseHeaderCellEvent(int col, bool action)
        {
            int idTraining = m_xAxis.GetDbId(col);
            Debug.Assert(idTraining > 0);
            Training training = Globals.DatabaseWrapper.TrainingFromId(idTraining);

            if (action)
            {
                ReportForm newForm = new ReportTraining(idTraining);
                newForm.Show();
                return String.Empty;
            }
            else
            {
                return $"Klicken für Details zu diesem Training";
            }
        }

        protected override string MouseMainDataAreaCellEvent(int row, int col, bool action)
            => String.Empty;

        protected override string MouseKeyCellEvent(int row, bool action)
            => MouseMemberKeyCellEvent(row, action);
    }
}
