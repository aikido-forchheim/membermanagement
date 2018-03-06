using System;
using System.Drawing;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportGraduationList : ReportBase
    {
        private AxisTypeMember P_axisTypeMember { get; set; }

        public ReportGraduationList()
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

            P_axisTypeMember = new AxisTypeMember(new DateTime(0), new DateTime(0));
            ReportFormPopulate();
        }

        private void ColorizeImportantDates( DataGridViewRow row, BestGraduation bestGrad )
        {

            if ((bestGrad.P_yearsOfMembership % 10 == 0) || (bestGrad.P_yearsOfMembership % 25 == 0))
                row.Cells["AVF-entry"].Style.ForeColor = Color.Red;

            if (bestGrad.P_datumMinNextGrad < DateTime.Now)
                row.Cells["dateNext"].Style.ForeColor = Color.Green;

            if (bestGrad.P_TrainngsDone >= bestGrad.P_trainingsNeeded)
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

        protected override void ReportFormPopulate()
        {
            DateTime           datValidData = Globals.DatabaseWrapper.GetStartValidData();
            BestGraduationList gradList     = new BestGraduationList();

            int gradIdLast = 0;
            foreach (BestGraduation bestGrad in gradList.P_listBestGraduation)
            {
                Mitglied member = Globals.DatabaseWrapper.MitgliedFromId(bestGrad.P_memberId);
                string   sGrad  = DisplayStringGraduation( ref gradIdLast, Globals.DatabaseWrapper.GraduierungFromId(bestGrad.P_graduierung));

                string strTrainingsNeeded = String.Empty;
                string strTrainingsDone   = String.Empty;
                string strDateNext        = String.Empty;
                if (bestGrad.P_graduierung >= 7) // index of 6. Kyu is 7
                {
                    strTrainingsNeeded = $" ({bestGrad.P_trainingsNeeded})";
                    strTrainingsDone   = (bestGrad.P_fAllTrainingsInDb ? "  " : "> ") + $"{ bestGrad.P_TrainngsDone }";
                    strDateNext        = Globals.Format(bestGrad.P_datumMinNextGrad);
                }

                P_dataGridView.Rows.Add
                (
                    sGrad,
                    member.Nachname,
                    member.Vorname,
                    member.Id,
                    Globals.Format( member.Geburtsdatum ),
                    member.Geburtsort,
                    Globals.DatabaseWrapper.BK_Text(member),
                    member.AikidoBeginn,
                    Globals.Format(member.Eintritt.Value),
                    Globals.Format(bestGrad.P_datumGraduierung),
                    strDateNext,
                    strTrainingsDone + strTrainingsNeeded
                );

                ColorizeImportantDates(P_dataGridView.Rows[P_dataGridView.Rows.Count - 1], bestGrad); 
            }
//            ExcelExport e = new ExcelExport();
//            e.Export2Excel(P_dataGridView, 2, 1, "Graduierungsliste" );
        }

        protected override string MouseCellEvent(int row, int col, bool action)
        {
            if (row >= 0)
            {
                int      idMember     = (int)P_dataGridView.Rows[row].Cells["memberId"].Value;
                string   strDateGrad  = P_dataGridView.Rows[row].Cells["dateGrad"].Value.ToString();
                DateTime dateGrad     = DateTime.Parse(strDateGrad);
                DateTime datValidData = Globals.DatabaseWrapper.GetStartValidData();
                DateTime dateStart    = (datValidData <= dateGrad) ? dateGrad : datValidData;
                return action
                    ? ReportMain.SwitchToPanel(new ReportMonthsVsCourses(dateStart, DateTime.Now, idMember))
                    : $"Klicken für Details zu Mitglied\n" + P_axisTypeMember.GetFullDesc(idMember);
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
