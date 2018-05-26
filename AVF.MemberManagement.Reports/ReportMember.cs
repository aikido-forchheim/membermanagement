using System;
using System.Drawing;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMember : ReportBase
    {
        private Mitglied     m_member;
        private DataGridView m_dgvPersonalData;

        public ReportMember(Mitglied member)
        {
            InitializeReportBase(); // creates DataGridView ...
            
            m_member = member;

            m_dgvPersonalData = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)(m_dgvPersonalData)).BeginInit();
            m_dgvPersonalData.AllowUserToAddRows = false;
            m_dgvPersonalData.AllowUserToDeleteRows = false;
            m_dgvPersonalData.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            m_dgvPersonalData.EnableHeadersVisualStyles = false;
            m_dgvPersonalData.Location = new Point(0, 50);
            m_dgvPersonalData.MultiSelect = true;
            m_dgvPersonalData.Name = "m_dgvPersonalData";
            m_dgvPersonalData.RowHeadersVisible = false;
            m_dgvPersonalData.RowHeadersWidth = 20;
            m_dgvPersonalData.RowTemplate.Height = 28;
            m_dgvPersonalData.Size = new Size(150, 400);
            m_dgvPersonalData.TabIndex = 0;
            m_dgvPersonalData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            m_dgvPersonalData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            m_dgvPersonalData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            m_dgvPersonalData.ColumnHeadersVisible = false;
            m_dgvPersonalData.DefaultCellStyle.SelectionBackColor = m_dgvPersonalData.DefaultCellStyle.BackColor;
            m_dgvPersonalData.DefaultCellStyle.SelectionForeColor = m_dgvPersonalData.DefaultCellStyle.ForeColor;
            m_dgvPersonalData.Columns.Add("item", "");
            m_dgvPersonalData.Columns.Add("value", "");
            m_dgvPersonalData.Rows.Add("Nachname:", member.Nachname);
            m_dgvPersonalData.Rows.Add("Vorname:", member.Vorname);
            m_dgvPersonalData.Rows.Add("Geburtsdatum:", Globals.Format(member.Geburtsdatum));
            m_dgvPersonalData.Rows.Add("Beruf:", "");
            m_dgvPersonalData.Rows.Add("Straße:", "");
            m_dgvPersonalData.Rows.Add("PLZ, Wohnort:", "");
            m_dgvPersonalData.Rows.Add("Telefon Festnetz:", member.Festnetz);
            m_dgvPersonalData.Rows.Add("Telefon mobil:", member.Mobil);
            m_dgvPersonalData.Rows.Add("Email1:", member.Email1);
            m_dgvPersonalData.Rows.Add("Email2:", member.Email2);
            m_dgvPersonalData.Rows.Add("Eintritt am:", Globals.Format(member.Eintritt));
            m_dgvPersonalData.Rows.Add("Beitragsklasse:", Globals.DatabaseWrapper.BK_Text(member));
            m_dgvPersonalData.Rows.Add("Aikido seit:", member.AikidoBeginn);
            ClientSize = new Size(839, 756);
            ((System.ComponentModel.ISupportInitialize)(m_dgvPersonalData)).EndInit();
            SizeDataGridView(m_dgvPersonalData);
            Controls.Add(m_dgvPersonalData);

            P_dataGridView.Location = new Point(0, 420);
            P_dataGridView.Columns.Add("graduation", "Grad");
            P_dataGridView.Columns.Add("dateGrad", "Datum");
            P_dataGridView.Columns.Add("examinant", "Prüfer");
            P_dataGridView.Columns.Add("dateNext", "Wartezeit");
            P_dataGridView.Columns.Add("nrTrainingsParticipations", "Trainings");

            ReportFormPopulate(null);
        }

        protected override void ReportFormPopulate(Action<String> tick)
        {
            Examinations m_examinations = new Examinations();
            foreach (Examination exam in m_examinations.GetSortedListOfExaminations(m_member))
            {
                P_dataGridView.Rows.Add
                (
                    exam.GraduationText(),
                    exam.GraduationDate(),
                    exam.Examinant(),
                    exam.WaitTime(),
                    exam.NrOfTrainings()
                );
            }
        }

        protected override string MouseCellEvent(int row, int col, MouseButtons buttons, bool action)
        {
            return String.Empty;
        }
    }
}
