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
            P_labelZeitraum = new System.Windows.Forms.Label();
            P_labelZeitraum.AutoSize = true;
            P_labelZeitraum.Font = P_font;
            P_labelZeitraum.Location = new System.Drawing.Point(500, 30);
            P_labelZeitraum.Name = "P_labelZeitraum";
            P_labelZeitraum.TabIndex = 1;
            // 
            // P_labelMember
            // 
            P_labelMember = new System.Windows.Forms.Label();
            P_labelMember.AutoSize = true;
            P_labelMember.Font = P_font;
            P_labelMember.Location = new System.Drawing.Point(20, 70);
            P_labelMember.Name = "P_labelMember";
            P_labelMember.TabIndex =2;
            // 
            // P_labelKurs
            // 
            P_labelKurs = new System.Windows.Forms.Label();
            P_labelKurs.AutoSize = true;
            P_labelKurs.Font = P_font;
            P_labelKurs.Location = new System.Drawing.Point(20, 100);
            P_labelKurs.Name = "P_labelKurs";
            P_labelKurs.TabIndex = 3;
            // 
            // P_labelMonat
            // 
            P_labelMonat = new System.Windows.Forms.Label();
            P_labelMonat.AutoSize = true;
            P_labelMonat.Font = P_font;
            P_labelMonat.Location = new System.Drawing.Point(300, 70);
            P_labelMonat.Name = "P_labelTrainer";
            P_labelMonat.TabIndex = 5;
            // 
            // P_labelTrainer
            // 
            P_labelTrainer = new System.Windows.Forms.Label();
            P_labelTrainer.AutoSize = true;
            P_labelTrainer.Font = P_font;
            P_labelTrainer.Location = new System.Drawing.Point(500, 70);
            P_labelTrainer.Name = "P_labelTrainer";
            P_labelTrainer.TabIndex = 6;
            // 
            // P_yearSelector
            // 
            P_yearSelector = new System.Windows.Forms.NumericUpDown();
            P_yearSelector.Location = new System.Drawing.Point(20, 140);
            P_yearSelector.Maximum = new decimal(new int[] {100, 0, 0, 0});
            P_yearSelector.Minimum = new decimal(new int[] {  0, 0, 0, 0});
            P_yearSelector.Name = "yearSelector";
            P_yearSelector.Size = new System.Drawing.Size(120, 26);
            P_yearSelector.TabIndex = 13;
            P_yearSelector.Value = new decimal(new int[] {0, 0, 0, 0});
            // 
            // ReportForm
            // 
            //            AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            //            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1344, 712);
            Controls.Add(P_labelTrainer);
            Controls.Add(P_labelZeitraum);
            Controls.Add(P_labelMember);
            Controls.Add(P_labelMonat);
            Controls.Add(P_labelKurs);
            Controls.Add(P_yearSelector);
            Name = "ReportForm";
            Text = "AVF Mitgliederdatenbank";

            ResumeLayout();
        }

        protected Label P_labelZeitraum;
        protected Label P_labelMember;
        protected Label P_labelMonat;
        protected Label P_labelKurs;
        protected Label P_labelTrainer;
        protected NumericUpDown P_yearSelector;
    }
}