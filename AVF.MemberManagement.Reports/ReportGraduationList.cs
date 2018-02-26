﻿using System;
using System.Drawing;
using System.Windows.Forms;
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
            //m_dataGridView.Columns.Add("birthplace", "Geburtsort");
            m_dataGridView.Columns.Add("memberClass", "Bei-\ntrags-\nklasse");
            m_dataGridView.Columns.Add("yearOfAikidoBegin", "Jahr des\nAikido-\nBeginns");
            m_dataGridView.Columns.Add("AVF-entry", "Eintrittsdatum");
            m_dataGridView.Columns.Add("dateGrad", "Datum der\nletzten\nGraduierung");
            m_dataGridView.Columns.Add("dateNext", "Mindestwarte-\nzeit für eine\nHöhergraduie\nrung erfüllt ab");
            m_dataGridView.Columns.Add("nrTrainingsParticipations", "Trainings\nseit der\nletzten\nGraduierung");

            m_dataGridView.RowHeadersVisible = false;
            m_dataGridView.AllowUserToAddRows = false;
            m_dataGridView.EnableHeadersVisualStyles = false;
            m_dataGridView.Location = new System.Drawing.Point(10, 100);

            foreach (DataGridViewColumn cols in m_dataGridView.Columns)
            {
                cols.SortMode = DataGridViewColumnSortMode.NotSortable;
                cols.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
/*
            Color orange = Color.FromArgb(255, 204, 153);
            Color green  = Color.FromArgb(204, 255, 204);
            m_dataGridView.Columns["graduation"].HeaderCell.Style.BackColor = orange;
            m_dataGridView.Columns["surname"].HeaderCell.Style.BackColor = green;
            m_dataGridView.Columns["firstName"                ].HeaderCell.Style.BackColor = green;
            m_dataGridView.Columns["memberId"].HeaderCell.Style.BackColor = orange;
            m_dataGridView.Columns["birthDate"                ].HeaderCell.Style.BackColor = green;
            //m_dataGridView.Columns["birthplace"               ].HeaderCell.Style.BackColor = green;
            m_dataGridView.Columns["memberClass"              ].HeaderCell.Style.BackColor = orange;
            m_dataGridView.Columns["yearOfAikidoBegin"        ].HeaderCell.Style.BackColor = green;
            m_dataGridView.Columns["AVF-entry"                ].HeaderCell.Style.BackColor = orange;
            m_dataGridView.Columns["dateGrad"                 ].HeaderCell.Style.BackColor = green;
            m_dataGridView.Columns["dateNext"                 ].HeaderCell.Style.BackColor = orange;
            m_dataGridView.Columns["nrTrainingsParticipations"].HeaderCell.Style.BackColor = green;
*/
            //            m_dataGridView.Columns["graduation"].HeaderCell.Style.Font = new System.Drawing.Font("Comic Sans MS", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            m_dataGridView.DefaultCellStyle.Font = new Font("Comic Sans MS", 11);
        }

        private void ColorizeImportantDates( DataGridViewRow row, BestGraduation bestGrad )
        {

            if ((bestGrad.m_yearsOfMembership % 10 == 0) || (bestGrad.m_yearsOfMembership % 25 == 0))
                row.Cells["AVF-entry"].Style.ForeColor = Color.Red;

            if (bestGrad.m_datumMinNextGrad < DateTime.Now)
                row.Cells["dateNext"].Style.ForeColor = Color.Green;

            if (bestGrad.m_TrainngsDone >= bestGrad.m_trainingsNeeded)
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
            foreach (BestGraduation bestGrad in gradList.m_listBestGraduation)
            {
                Mitglied member = Globals.DatabaseWrapper.MitgliedFromId(bestGrad.m_memberId);
                string   sGrad  = DisplayStringGraduation( ref gradIdLast, Globals.DatabaseWrapper.GraduierungFromId(bestGrad.m_graduierung));

                string strTrainingsNeeded = String.Empty;
                string strTrainingsDone   = String.Empty;
                string strDateNext        = String.Empty;
                if (bestGrad.m_graduierung >= 7) // index of 6. Kyu is 7
                {
                    strTrainingsNeeded = $" ({bestGrad.m_trainingsNeeded})";
                    strTrainingsDone   = (bestGrad.m_fAllTrainingsInDb ? "  " : "> ") + $"{ bestGrad.m_TrainngsDone }";
                    strDateNext        = Globals.Format(bestGrad.m_datumMinNextGrad);
                }

                m_dataGridView.Rows.Add
                (
                    sGrad,
                    member.Nachname,
                    member.Vorname,
                    member.Id,
                    Globals.Format( member.Geburtsdatum ),
//                    member.Geburtsort,
                    Globals.DatabaseWrapper.BK_Text(member),
                    member.AikidoBeginn,
                    Globals.Format(member.Eintritt.Value),
                    Globals.Format(member.Geburtsdatum),
                    strDateNext,
                    strTrainingsDone + strTrainingsNeeded
                );

                ColorizeImportantDates(m_dataGridView.Rows[m_dataGridView.Rows.Count - 1], bestGrad); 
            }
//            ExcelExport e = new ExcelExport();
//            e.Export2Excel(m_dataGridView, 2, 1, "Graduierungsliste" );
        }


        protected override string MouseCellEvent(int row, int col, bool action)
        {
            if (row >= 0)
            {
                int idMember = (int)m_dataGridView.Rows[row].Cells["memberId"].Value;
                string strDateGrad = m_dataGridView.Rows[row].Cells["dateGrad"].Value.ToString();
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
