using System;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsCourses : ReportForm
    {
        public ReportMemberVsCourses( DateTime datStart, DateTime datEnd )
        {
            CreateModel
            (
                datStart, datEnd,
                new HorizontalAxisCourses(), 
                new VerticalAxisMembers(),
                filter: tn => true
            );

            m_label1.Text = "Überblick Trainingsteilnahme";
            m_label2.Text = Globals.GetTimeRangeDescription(datStart, datEnd);
        }

        protected override string MouseCellEvent( int row, int col, bool action )
        {
            ReportForm newForm  = null;
            string     helpText = String.Empty;

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
            else // data row
            {
                int      idMember = (int)m_dataGridView[2, row].Value;
                Mitglied member   = Globals.DatabaseWrapper.MitgliedFromId(idMember);
                if ( ColIsInMainArea( col ) )
                {
                    if (action)
                        newForm = new ReportMemberVsTrainings(m_datStart, m_datEnd, m_xAxis.GetDbId(col));
                    else
                        helpText = $"Klicken für Details zur Teilnahme von\n {member.Vorname} {member.Nachname} am Kurs\n" + Globals.GetCourseDescription(m_xAxis.GetDbId(col));
                }
                else // key or summary 
                {
                    if (action)
                        newForm = new ReportWeekVsCourses(m_datStart, m_datEnd, idMember);
                    else
                        helpText = $"Klicken für Details zu Mitglied\n{member.Vorname} {member.Nachname}";
                }
            }

            if (newForm != null)
                newForm.Show();

            return helpText;
        }
    }
}
