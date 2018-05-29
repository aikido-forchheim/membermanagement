using System;
using System.Drawing;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportGraduationList : ReportBase
    {
        private AxisTypeMember m_axisTypeMember;
        private Examinations   m_examinations;
//        private ContextMenu    m_contextMenue;

        public ReportGraduationList(Action<String> tick)
        {
            InitializeReportBase(); // creates DataGridView ...

            P_dataGridView.Columns.Add("graduation", "Grad");
            P_dataGridView.Columns.Add("surname", "Name");
            P_dataGridView.Columns.Add("firstName", "Vorname");
            P_dataGridView.Columns.Add("memberId", "lfd.\nNr.");
            P_dataGridView.Columns.Add("birthDate", "Geburts-\ndatum");
            P_dataGridView.Columns.Add("birthplace", "Geburtsort");
            P_dataGridView.Columns.Add("memberClass", "Bei-\ntrags-\nklasse");
            P_dataGridView.Columns.Add("yearOfAikidoBegin", "Jahr des\nAikido-\nBeginns");
            P_dataGridView.Columns.Add("AVF-entry", "Eintrittsdatum");
            P_dataGridView.Columns.Add("dateGrad", "Datum der\nletzten\nGraduierung");
            P_dataGridView.Columns.Add("dateNext", "Mindestwarte-\nzeit für eine\nHöhergraduie\nrung erfüllt ab");
            P_dataGridView.Columns.Add("nrTrainingsParticipations", "Trainings\nseit der\nletzten\nGraduierung");

            P_dataGridView.RowHeadersVisible = false;
            P_dataGridView.AllowUserToAddRows = false;
            P_dataGridView.EnableHeadersVisualStyles = false;

            foreach (DataGridViewColumn cols in P_dataGridView.Columns)
            {
                cols.SortMode = DataGridViewColumnSortMode.NotSortable;
                cols.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            P_dataGridView.DefaultCellStyle.Font = new Font("Comic Sans MS", 11);

            m_axisTypeMember = new AxisTypeMember(new ReportDescriptor());
            m_examinations = new Examinations();
            //            m_contextMenue = new ContextMenu();
            //            m_contextMenue.MenuItems.Add("Item 1", new EventHandler(delegate (object sender, System.EventArgs e) { }));
            //            m_contextMenue.MenuItems.Add("Item 2", new EventHandler(delegate (object sender, System.EventArgs e) { }));

            P_dataGridView.CellMouseClick += new DataGridViewCellMouseEventHandler
            (
                delegate (object sender, DataGridViewCellMouseEventArgs e)
                {
                    MouseCellEvent(e.RowIndex, e.ColumnIndex, e.Button, action: true);
                }
            );

            ReportFormPopulate(tick);
        }

        private void ColorizeImportantDates( DataGridViewRow row, Examination bestGrad )
        {
            int yearsOfMembership = Globals.DatabaseWrapper.YearsOfMembership(bestGrad.P_memberId);

            if ((yearsOfMembership > 0) && ((yearsOfMembership % 10 == 0) || (yearsOfMembership % 25 == 0)))
                row.Cells["AVF-entry"].Style.ForeColor = Color.Red;

            if (bestGrad.P_datumMinNextGrad < DateTime.Now)
                row.Cells["dateNext"].Style.ForeColor = Color.Green;

            if (bestGrad.P_nrOfTrainingsSinceLastExam >= bestGrad.P_nrOfTrainingsNeeded)
                row.Cells["nrTrainingsParticipations"].Style.ForeColor = Color.Green;
        }

        private string DisplayStringGraduation( ref int gradIdLast, Graduierung gradActual )  // Display graduation only in first row of occurence
        {
            string sGrad = "";                    
            if (gradActual.Id != gradIdLast)
            {
                sGrad = $"{gradActual.Bezeichnung} ";
                if (gradActual.Id > 1)
                    sGrad += $"({gradActual.Japanisch}) ";
                gradIdLast = gradActual.Id;
            }

            return sGrad;
        }

        private void ReportFormPopulate(Action<String> tick)
        {
            DateTime datValidData = Globals.DatabaseWrapper.GetStartValidData();
            var      gradList     = m_examinations.GetBestGraduationList(tick);

            int firstGradWithDefinedWaitingTime = 7;   // index of 6. Kyu is 7

            int gradIdLast = 0;
            foreach (Examination exam in gradList)
            {
                Mitglied member = Globals.DatabaseWrapper.MitgliedFromId(exam.P_memberId);
                string   sGrad  = DisplayStringGraduation( ref gradIdLast, Globals.DatabaseWrapper.GraduierungFromId(exam.P_gradId));

                string strTrainings = String.Empty;
                string strDateNext  = String.Empty;
                if (exam.P_gradId >= firstGradWithDefinedWaitingTime) 
                {
                    strTrainings = exam.NrOfTrainings();
                    strDateNext = Globals.Format(exam.P_datumMinNextGrad);
                }

                P_dataGridView.Rows.Add
                (
                    sGrad,
                    member.Nachname,
                    member.Vorname,
                    member.Id,
                    Globals.Format(member.Geburtsdatum),
                    member.Geburtsort,
                    Globals.DatabaseWrapper.BK_Text(member),
                    member.AikidoBeginn,
                    Globals.Format(member.Eintritt.Value),
                    Globals.Format(exam.P_range.P_datStart),
                    strDateNext,
                    strTrainings
                );

                ColorizeImportantDates(P_dataGridView.Rows[P_dataGridView.RowCount - 1], exam); 
            }
        }

        private string MouseCellEvent(int row, int col, MouseButtons buttons, bool action)
        {
            if (row >= 0)
            {
                var relativeMousePosition = P_dataGridView.PointToClient(Cursor.Position);
//                m_contextMenue.Show(P_dataGridView, relativeMousePosition);
                int idMember = (int)P_dataGridView.Rows[row].Cells["memberId"].Value;
                if (col < 3)
                {
                    return ReportMain.P_formMain.SwitchToPanel(new ReportMember(Globals.DatabaseWrapper.MitgliedFromId(idMember)));
                }
                else
                {
                    string strDateGrad = P_dataGridView.Rows[row].Cells["dateGrad"].Value.ToString();
                    DateTime dateGrad = DateTime.Parse(strDateGrad);
                    DateTime datValidData = Globals.DatabaseWrapper.GetStartValidData();
                    DateTime dateStart = (datValidData <= dateGrad) ? dateGrad : datValidData;
                    TimeRange timeRange = new TimeRange(dateStart, DateTime.Now);
                    return action
                        ? ReportMain.P_formMain.NewTrainingsParticipationPanel(null, typeof(AxisTypeCourse), typeof(AxisTypeMonth), timeRange, idMember: idMember)
                        : $"Klicken für Details zu Mitglied\n" + m_axisTypeMember.GetFullDesc(idMember, Globals.TEXT_ORIENTATION.HORIZONTAL);
                }
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
