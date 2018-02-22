using System;
using System.Windows.Forms;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportGraduationList : ReportBase
    {
        public ReportGraduationList()
        {
            m_dataGridView = new DataGridView();

            m_dataGridView.Columns.Add("graduation", "Grad");
            m_dataGridView.Columns.Add("surname", "Name");
            m_dataGridView.Columns.Add("firstName", "Vorname");
            m_dataGridView.Columns.Add("memberId", "lfd.\nNr.");
            m_dataGridView.Columns.Add("birthDate", "Geburts-\ndatum");
            m_dataGridView.Columns.Add("birthplace", "Geburtsort");
            m_dataGridView.Columns.Add("memberClass", "Bei-\ntrags-\nklasse");
            m_dataGridView.Columns.Add("yearOfAikidoBegin", "Jahr des\nAikido-\nBeginns");
            m_dataGridView.Columns.Add("dateGrad", "Datum der\nletzten\nGraduierung");
            m_dataGridView.Columns.Add("dateNext", "Mindestwarte-\nzeit für eine\nHöhergraduie\nrung erfüllt ab");
            m_dataGridView.Columns.Add("nrTrainingsParticipations", "Trainings\nseit der\nletzten\nGraduierung");

            m_dataGridView.Size = new System.Drawing.Size(1345, 712);

            foreach ( DataGridViewColumn cols in m_dataGridView.Columns )
            {
                cols.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            this.PerformLayout();
        }

        protected override void ReportFormPopulate()
        {
            DateTime           datValidData = Globals.DatabaseWrapper.GetStartValidData( );
            BestGraduationList gradList     = new BestGraduationList();

            List<TrainingsTeilnahme> tnList = Globals.DatabaseWrapper.TrainingsTeilnahme(datValidData, DateTime.Now);

            int iGradIdLast = 0;
            foreach (BestGraduation bestGrad in gradList.m_listBestGraduation )
            {
                Graduierung gradActual = Globals.DatabaseWrapper.GraduierungFromId( bestGrad.m_graduierung );
                Mitglied    member     = Globals.DatabaseWrapper.MitgliedFromId   ( bestGrad.m_memberId );
                DateTime    dateGrad   = bestGrad.m_datumGraduierung;
                DateTime    dateNext   = bestGrad.m_datumMinNextGrad;

                // determine number of training participations since last graduation 

                List<TrainingsTeilnahme> tnListMember   = Globals.DatabaseWrapper.Filter(tnList, member.Id);        // training participations of member
                List<TrainingsTeilnahme> tnListRelevant = Globals.DatabaseWrapper.Filter(tnListMember, dateGrad);   // training participations since last graduation

                string sGrad = "";
                if (gradActual.Id != iGradIdLast)
                {
                    sGrad = $"{gradActual.Bezeichnung} ";
                    if (gradActual.Id > 1)
                        sGrad += $"({gradActual.Japanisch}) ";
                    iGradIdLast = gradActual.Id;
                }

                string strTrainingsNeeded = $" ({bestGrad.m_trainingsNeeded})";
                string strTrainingsDone   = ((dateGrad < datValidData) ? "> " : "  ") + $"{ tnListRelevant.Count }";

                m_dataGridView.Rows.Add
                (
                    sGrad,
                    member.Nachname,
                    member.Vorname,
                    member.Id,
                    $"{ member.Geburtsdatum:dd.MM.yyyy} ",
                    member.Geburtsort,
                    Globals.DatabaseWrapper.BK_Text(member),
                    member.AikidoBeginn,
                    $"{ dateGrad:dd.MM.yyyy} ",
                    $"{ dateNext:dd.MM.yyyy} ",
                    strTrainingsDone + strTrainingsNeeded
                );
            }

            /*
                        foreach ( DataGridViewRow row in m_dataGridView.Rows )
                        {
                            if ( row.Cells["dateNext"].Value <=  )
                        }
            */
        }
    }
}
