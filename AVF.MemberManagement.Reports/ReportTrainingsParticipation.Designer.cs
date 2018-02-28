using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    partial class ReportTrainingsParticipation
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

        protected override void InitializeComponent()
        {
            SuspendLayout();
            base.InitializeComponent();

            // 
            // m_labelZeitraum
            // 
            this.m_labelZeitraum = new System.Windows.Forms.Label();
            this.m_labelZeitraum.AutoSize = true;
            this.m_labelZeitraum.Font = m_font;
            this.m_labelZeitraum.Location = new System.Drawing.Point(500, 20);
            this.m_labelZeitraum.Name = "m_labelZeitraum";
            this.m_labelZeitraum.TabIndex = 2;
            // 
            // m_Info0
            // 
            this.m_Info0 = new System.Windows.Forms.Label();
            this.m_Info0.AutoSize = true;
            this.m_Info0.Font = m_font;
            this.m_Info0.Location = new System.Drawing.Point(20, 70);
            this.m_Info0.Name = "m_Info0";
            this.m_Info0.TabIndex = 3;
            // 
            // m_Info1
            // 
            this.m_Info1 = new System.Windows.Forms.Label();
            this.m_Info1.AutoSize = true;
            this.m_Info1.Font = m_font;
            this.m_Info1.Location = new System.Drawing.Point(500, 70);
            this.m_Info1.Name = "m_Info1";
            this.m_Info1.TabIndex = 4;
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1344, 712);
            this.Controls.Add(this.m_Info1);
            this.Controls.Add(this.m_labelZeitraum);
            this.Controls.Add(this.m_Info0);
            this.Name = "ReportForm";
            this.Text = "AVF Mitgliederdatenbank";

            ResumeLayout();
        }

        protected Label m_labelZeitraum;
        protected Label m_Info0;
        protected Label m_Info1;
    }
}