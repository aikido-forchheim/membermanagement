using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    partial class ReportForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_dataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.m_dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // m_dataGridView
            // 
            this.m_dataGridView.AllowUserToAddRows = false;
            this.m_dataGridView.AllowUserToDeleteRows = false;
            this.m_dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dataGridView.EnableHeadersVisualStyles = false;
            this.m_dataGridView.Location = new System.Drawing.Point(0, 491);
            this.m_dataGridView.MultiSelect = false;
            this.m_dataGridView.Name = "m_dataGridView";
            this.m_dataGridView.RowHeadersVisible = false;
            this.m_dataGridView.RowTemplate.Height = 28;
            this.m_dataGridView.Size = new System.Drawing.Size(1377, 500);
            this.m_dataGridView.TabIndex = 0;
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1377, 991);
            this.Controls.Add(this.m_dataGridView);
            this.Name = "ReportForm";
            this.Text = "PopupReport";
            ((System.ComponentModel.ISupportInitialize)(this.m_dataGridView)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        protected DataGridView m_dataGridView;
    }
}