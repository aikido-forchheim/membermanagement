namespace AVF.MemberManagement.Reports
{
    partial class ReportMain
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.Trainingsteilnahme = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.Graduierungsliste = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(181, 441);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(417, 51);
            this.progressBar1.Step = 6;
            this.progressBar1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(193, 346);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(255, 32);
            this.label1.TabIndex = 2;
            this.label1.Text = "Lade Datenbank ...";
            // 
            // Trainingsteilnahme
            // 
            this.Trainingsteilnahme.Enabled = false;
            this.Trainingsteilnahme.Location = new System.Drawing.Point(154, 71);
            this.Trainingsteilnahme.Name = "Trainingsteilnahme";
            this.Trainingsteilnahme.Size = new System.Drawing.Size(171, 52);
            this.Trainingsteilnahme.TabIndex = 3;
            this.Trainingsteilnahme.Text = "Trainingsteilnahme";
            this.Trainingsteilnahme.UseVisualStyleBackColor = true;
            this.Trainingsteilnahme.Click += new System.EventHandler(this.Trainingsteilnahme_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(195, 395);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(336, 27);
            this.label2.TabIndex = 0;
            // 
            // Graduierungsliste
            // 
            this.Graduierungsliste.Enabled = false;
            this.Graduierungsliste.Location = new System.Drawing.Point(405, 71);
            this.Graduierungsliste.Name = "Graduierungsliste";
            this.Graduierungsliste.Size = new System.Drawing.Size(171, 52);
            this.Graduierungsliste.TabIndex = 4;
            this.Graduierungsliste.Text = "Graduierungsliste";
            this.Graduierungsliste.UseVisualStyleBackColor = true;
            // 
            // ReportMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 756);
            this.Controls.Add(this.Graduierungsliste);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Trainingsteilnahme);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Name = "ReportMain";
            this.Text = "ReportMain";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Trainingsteilnahme;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Graduierungsliste;
    }
}