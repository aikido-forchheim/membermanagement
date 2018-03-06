using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    public partial class ReportTrainingsParticipation : ReportBase
    {
        protected void InitializeReportTrainingsParticipation()
        {
            SuspendLayout();
            InitializeReportBase();

            // 
            // P_labelZeitraum
            // 
            this.P_labelZeitraum = new System.Windows.Forms.Label();
            this.P_labelZeitraum.AutoSize = true;
            this.P_labelZeitraum.Font = P_font;
            this.P_labelZeitraum.Location = new System.Drawing.Point(500, 30);
            this.P_labelZeitraum.Name = "P_labelZeitraum";
            this.P_labelZeitraum.TabIndex = 2;
            // 
            // P_Info0
            // 
            this.P_Info0 = new System.Windows.Forms.Label();
            this.P_Info0.AutoSize = true;
            this.P_Info0.Font = P_font;
            this.P_Info0.Location = new System.Drawing.Point(20, 70);
            this.P_Info0.Name = "P_Info0";
            this.P_Info0.TabIndex = 3;
            // 
            // P_Info1
            // 
            this.P_Info1 = new System.Windows.Forms.Label();
            this.P_Info1.AutoSize = true;
            this.P_Info1.Font = P_font;
            this.P_Info1.Location = new System.Drawing.Point(500, 70);
            this.P_Info1.Name = "P_Info1";
            this.P_Info1.TabIndex = 4;
            // 
            // ReportForm
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1344, 712);
            this.Controls.Add(this.P_Info1);
            this.Controls.Add(this.P_labelZeitraum);
            this.Controls.Add(this.P_Info0);
            this.Name = "ReportForm";
            this.Text = "AVF Mitgliederdatenbank";

            ResumeLayout();
        }

        protected Label P_labelZeitraum;
        protected Label P_Info0;
        protected Label P_Info1;
    }
}