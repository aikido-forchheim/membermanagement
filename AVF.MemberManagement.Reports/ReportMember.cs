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
            InitializeComponent();

            m_member = member;
            /*
                        m_dgvPersonalData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                        m_dgvPersonalData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                        m_dgvPersonalData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                        m_dgvPersonalData.DefaultCellStyle.SelectionBackColor = m_dgvPersonalData.DefaultCellStyle.BackColor;
                        m_dgvPersonalData.DefaultCellStyle.SelectionForeColor = m_dgvPersonalData.DefaultCellStyle.ForeColor;
            */
            foreach (DataGridViewColumn cols in m_dgvPersonalData.Columns)
                cols.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            Wohnung wohnung = Globals.DatabaseWrapper.Wohnung(member);
            m_dgvPersonalData.Columns.Add("item", "");
            m_dgvPersonalData.Columns.Add("value", "");
            m_dgvPersonalData.Rows.Add("Nachname:", member.Nachname);
            m_dgvPersonalData.Rows.Add("Vorname:", member.Vorname);
            m_dgvPersonalData.Rows.Add("Geburtsdatum:", Globals.Format(member.Geburtsdatum));
            m_dgvPersonalData.Rows.Add("Beruf:", "");
            m_dgvPersonalData.Rows.Add("Straße:",       (wohnung == null) ? "" : wohnung.Strasse + " " + wohnung.Hausnummer);
            m_dgvPersonalData.Rows.Add("PLZ, Wohnort:", (wohnung == null) ? "" : wohnung.PLZ + " " + wohnung.Ort);
            m_dgvPersonalData.Rows.Add("Telefon Festnetz:", member.Festnetz);
            m_dgvPersonalData.Rows.Add("Telefon mobil:", member.Mobil);
            m_dgvPersonalData.Rows.Add("Email1:", member.Email1);
            m_dgvPersonalData.Rows.Add("Email2:", member.Email2);
            m_dgvPersonalData.Rows.Add("Eintritt am:", Globals.Format(member.Eintritt));
            m_dgvPersonalData.Rows.Add("Beitragsklasse:", Globals.DatabaseWrapper.BK_Text(member));
            m_dgvPersonalData.Rows.Add("Aikido seit:", member.AikidoBeginn);
            //            ClientSize = new Size(839, 756);
            //            SizeDataGridView(m_dgvPersonalData);
            P_panel.Controls.Add(m_dgvPersonalData);
            //
            // P_dataGridView
            //
            P_dataGridView.Location = new Point(m_dgvPersonalData.Size.Width + 10, m_dgvPersonalData.Location.Y);
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

        private void InitializeComponent()
        {
            this.m_dgvPersonalData = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgvPersonalData)).BeginInit();
            P_panel.SuspendLayout();
            // 
            // m_dgvPersonalData
            // 
            this.m_dgvPersonalData.AllowUserToAddRows = false;
            this.m_dgvPersonalData.AllowUserToDeleteRows = false;
            this.m_dgvPersonalData.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.m_dgvPersonalData.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.m_dgvPersonalData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.m_dgvPersonalData.ColumnHeadersVisible = false;
            this.m_dgvPersonalData.EnableHeadersVisualStyles = false;
            this.m_dgvPersonalData.Location = new System.Drawing.Point(50, 100);
            this.m_dgvPersonalData.Name = "m_dgvPersonalData";
            this.m_dgvPersonalData.RowHeadersVisible = false;
            this.m_dgvPersonalData.Size = new System.Drawing.Size(240, 150);
            this.m_dgvPersonalData.TabIndex = 0;
            this.m_dgvPersonalData.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            ((System.ComponentModel.ISupportInitialize)(this.m_dgvPersonalData)).EndInit();
            P_panel.ResumeLayout(false);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
