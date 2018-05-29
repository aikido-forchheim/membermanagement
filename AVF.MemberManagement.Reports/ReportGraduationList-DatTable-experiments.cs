using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportGraduationList : ReportBase
    {
        DataTable m_dataTable;
        BindingSource m_bindingSource;

        public ReportGraduationList()
        {
            m_dataGridView = new DataGridView();

            m_dataTable = new DataTable();
            m_bindingSource = new BindingSource();


            m_dataTable.Columns.Add("graduation", typeof(String) );
            m_dataTable.Columns.Add("surname", typeof(String));
            m_dataTable.Columns.Add("firstName", typeof(String));
            m_dataTable.Columns.Add("memberId", typeof(int));
            m_dataTable.Columns.Add("birthDate", typeof(String));
            m_dataTable.Columns.Add("birthplace", typeof(String));
            m_dataTable.Columns.Add("memberClass", typeof(String));
            m_dataTable.Columns.Add("yearOfAikidoBegin", typeof(int));
            m_dataTable.Columns.Add("AVF-entry", typeof(String));
            m_dataTable.Columns.Add("dateGrad", typeof(String));
            m_dataTable.Columns.Add("dateNext", typeof(String));
            m_dataTable.Columns.Add("nrTrainingsParticipations", typeof(String));
/*
            m_dataGridView.Columns.Add("graduation", "Grad");
            m_dataGridView.Columns.Add("surname", "Name");
            m_dataGridView.Columns.Add("firstName", "Vorname");
            m_dataGridView.Columns.Add("memberId", "lfd.\nNr.");
            m_dataGridView.Columns.Add("birthDate", "Geburts-\ndatum");
            m_dataGridView.Columns.Add("birthplace", "Geburtsort");
            m_dataGridView.Columns.Add("memberClass", "Bei-\ntrags-\nklasse");
            m_dataGridView.Columns.Add("yearOfAikidoBegin", "Jahr des\nAikido-\nBeginns");
            m_dataGridView.Columns.Add("AVF-entry", "Eintrittsdatum");
            m_dataGridView.Columns.Add("dateGrad", "Datum der\nletzten\nGraduierung");
            m_dataGridView.Columns.Add("dateNext", "Mindestwarte-\nzeit für eine\nHöhergraduie\nrung erfüllt ab");
            m_dataGridView.Columns.Add("nrTrainingsParticipations", "Trainings\nseit der\nletzten\nGraduierung");
*/
            m_dataGridView.RowHeadersVisible = false;
            m_dataGridView.AllowUserToAddRows = false;
            m_dataGridView.EnableHeadersVisualStyles = false;
            m_dataGridView.Location = new System.Drawing.Point(10, 100);

            foreach (DataGridViewColumn cols in m_dataGridView.Columns)
            {
                cols.SortMode = DataGridViewColumnSortMode.NotSortable;
                cols.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            m_bindingSource.DataSource = m_dataTable;
            m_dataGridView.DataSource = m_bindingSource;

            m_dataGridView.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(DataBindingComplete);
        }

        private void DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            m_dataGridView.Columns["birthDate"].HeaderCell.Style.BackColor = Color.FromArgb(255, 204, 153); ;
        }

        protected override void ReportFormPopulate()
        {
            DateTime           datValidData = Globals.DatabaseWrapper.GetStartValidData();
            BestGraduationList gradList     = new BestGraduationList();

            int iGradIdLast = 0;
            foreach (BestGraduation bestGrad in gradList.m_listBestGraduation)
            {
                Graduierung gradActual = Globals.DatabaseWrapper.GraduierungFromId(bestGrad.m_graduierung);
                Mitglied    member     = Globals.DatabaseWrapper.MitgliedFromId(bestGrad.m_memberId);

                DateTime dateGrad = bestGrad.m_datumGraduierung;
                DateTime dateNext = bestGrad.m_datumMinNextGrad;

                bool     fAllRelevantTrainingsInDb = (datValidData <= dateGrad);
                DateTime dateStart = fAllRelevantTrainingsInDb ? dateGrad : datValidData;

                int nrOfTrainingsSinceLastGraduation = Globals.DatabaseWrapper.NrOfTrainingsSince(member.Id, dateStart);

                string sGrad = "";
                if (gradActual.Id != iGradIdLast)
                {
                    sGrad = $"{gradActual.Bezeichnung} ";
                    if (gradActual.Id > 1)
                        sGrad += $"({gradActual.Japanisch}) ";
                    iGradIdLast = gradActual.Id;
                }

                string strTrainingsNeeded;
                string strTrainingsDone;
                string strDateNext;
                if (bestGrad.m_graduierung <= 6) // index of 7. Kyu is 6
                {
                    strTrainingsNeeded = String.Empty;
                    strTrainingsDone = String.Empty;
                    strDateNext = String.Empty;
                }
                else
                {
                    strTrainingsNeeded = $" ({bestGrad.m_trainingsNeeded})";
                    strTrainingsDone   = (fAllRelevantTrainingsInDb ? "  " : "> ") + $"{ nrOfTrainingsSinceLastGraduation }";
                    strDateNext = $"{ dateNext:dd.MM.yyyy}";
                }

                m_dataTable.Rows.Add
                (
                    sGrad,
                    member.Nachname,
                    member.Vorname,
                    member.Id,
                    $"{ member.Geburtsdatum:dd.MM.yyyy}",
                    member.Geburtsort,
                    Globals.DatabaseWrapper.BK_Text(member),
                    member.AikidoBeginn,
                    $"{ member.Eintritt:dd.MM.yyyy}",
                    $"{ dateGrad:dd.MM.yyyy}",
                    strDateNext,
                    strTrainingsDone + strTrainingsNeeded
                );
                /*
               
                DataGridViewRow row = m_dataGridView.Rows[m_dataGridView.Rows.Count - 1];

                int yearsOfMembership = DateTime.Now.Year - member.Eintritt.Value.Year;
                if ( (yearsOfMembership == 10) || (yearsOfMembership == 20) || (yearsOfMembership == 25) || (yearsOfMembership == 30) )
                    row.Cells["AVF-entry"].Style.ForeColor = Color.Red;

                if (dateNext < DateTime.Now)
                    row.Cells["dateNext"].Style.ForeColor = Color.Green;

                if (nrOfTrainingsSinceLastGraduation >= bestGrad.m_trainingsNeeded)
                    row.Cells["nrTrainingsParticipations"].Style.ForeColor = Color.Green;
               */
            }

        }

        protected override string MouseCellEvent(int row, int col, bool action)
        {
            if (row >= 0)
            {
                int idMember = (int)m_dataTable.Rows[row]["memberId"];
                string strDateGrad = m_dataTable.Rows[row]["dateGrad"].ToString();
                DateTime dateGrad = DateTime.Parse(strDateGrad);
                return action
                    ? ReportTrainingsParticipation.Show(new ReportWeekVsCourses(dateGrad, DateTime.Now, idMember))
                    : $"Klicken für Details zu Mitglied\n" + Globals.GetMemberDescription(idMember);
            }
            else
            {
                return String.Empty;
            }

        }
    }
}
