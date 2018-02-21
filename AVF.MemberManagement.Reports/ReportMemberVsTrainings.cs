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
                (tn => Globals.DatabaseWrapper.KursIdFromTrainingId(tn.TrainingID) == idKurs)
            );

            m_label1.Text = Globals.GetCourseDescription(idKurs);
            m_label2.Text = Globals.GetTimeRangeDescription(datStart, datEnd);
        }

        protected override string FormatMatrixElement(int iValue) 
            => (iValue > 0) ? $"X" : " ";

        protected override string MouseCellEvent(int row, int col, bool action)
        {
            ReportForm newForm = null;
            string helpText = String.Empty;

            if (RowIsHeader(row))
            {
                if (ColIsInMainArea(col))
                {
                    int idTraining = m_xAxis.GetDbId(col);
                    Debug.Assert(idTraining > 0);
                    Training training = Globals.DatabaseWrapper.TrainingFromId(idTraining);
                    if (action)
                        newForm = new ReportTraining(idTraining);
                    else
                        return $"Klicken für Details zu diesem Training";
                }
                else // column header, key or summary columns
                {
                }
            }
            else // data row
            {
                if (ColIsInMainArea(col))
                {
                }
                else // key or summary 
                {
                    int idMember = (int)m_dataGridView[2, row].Value;
                    Mitglied member = Globals.DatabaseWrapper.MitgliedFromId(idMember);
                    if (action)
                        newForm = new ReportWeekVsCourses(m_datStart, m_datEnd, idMember);
                    else
                        return $"Klicken für Details zu Mitglied\n {member.Vorname} {member.Nachname}";
                }
            }

            if (newForm != null)
                newForm.Show();

            return helpText;
        }
    }
}
