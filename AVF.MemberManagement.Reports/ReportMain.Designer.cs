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
            this.Kurse = new System.Windows.Forms.Button();
            this.Graduierungsliste = new System.Windows.Forms.Button();
            this.TrainingsteilnahmeKurse = new System.Windows.Forms.Button();
            this.TrainingsteilnahmeMonate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
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
            // Kurse
            // 
            this.Kurse.Enabled = false;
            this.Kurse.Location = new System.Drawing.Point(380, 168);
            this.Kurse.Name = "Kurse";
            this.Kurse.Size = new System.Drawing.Size(171, 52);
            this.Kurse.TabIndex = 3;
            this.Kurse.Text = "Überblick Kurse";
            this.Kurse.UseVisualStyleBackColor = true;
            this.Kurse.Click += new System.EventHandler(this.Kurse_Click);
            // 
            // Graduierungsliste
            // 
            this.Graduierungsliste.Enabled = false;
            this.Graduierungsliste.Location = new System.Drawing.Point(156, 168);
            this.Graduierungsliste.Name = "Graduierungsliste";
            this.Graduierungsliste.Size = new System.Drawing.Size(171, 52);
            this.Graduierungsliste.TabIndex = 4;
            this.Graduierungsliste.Text = "Graduierungsliste";
            this.Graduierungsliste.UseVisualStyleBackColor = true;
            this.Graduierungsliste.Click += new System.EventHandler(this.Gradierungsliste_Click);
            // 
            // TrainingsteilnahmeKurse
            // 
            this.TrainingsteilnahmeKurse.Enabled = false;
            this.TrainingsteilnahmeKurse.Location = new System.Drawing.Point(156, 71);
            this.TrainingsteilnahmeKurse.Name = "TrainingsteilnahmeKurse";
            this.TrainingsteilnahmeKurse.Size = new System.Drawing.Size(171, 52);
            this.TrainingsteilnahmeKurse.TabIndex = 5;
            this.TrainingsteilnahmeKurse.Text = "Trainingsteilnahme Kurse";
            this.TrainingsteilnahmeKurse.UseVisualStyleBackColor = true;
            this.TrainingsteilnahmeKurse.Click += new System.EventHandler(this.Trainingsteilnahme_Kurse_Click);
            // 
            // TrainingsteilnahmeMonate
            // 
            this.TrainingsteilnahmeMonate.Enabled = false;
            this.TrainingsteilnahmeMonate.Location = new System.Drawing.Point(380, 71);
            this.TrainingsteilnahmeMonate.Name = "TrainingsteilnahmeMonate";
            this.TrainingsteilnahmeMonate.Size = new System.Drawing.Size(171, 52);
            this.TrainingsteilnahmeMonate.TabIndex = 6;
            this.TrainingsteilnahmeMonate.Text = "Trainingsteilnahme Monate";
            this.TrainingsteilnahmeMonate.UseVisualStyleBackColor = true;
            this.TrainingsteilnahmeMonate.Click += new System.EventHandler(this.Trainingsteilnahme_Monate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(187, 338);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 32);
            this.label1.TabIndex = 2;
            this.label1.Text = "Lade Datenbank";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(187, 390);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(224, 32);
            this.label2.TabIndex = 7;
            this.label2.Text = "................";
            // 
            // ReportMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 756);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TrainingsteilnahmeMonate);
            this.Controls.Add(this.TrainingsteilnahmeKurse);
            this.Controls.Add(this.Graduierungsliste);
            this.Controls.Add(this.Kurse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Name = "ReportMain";
            this.Text = "ReportMain";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button Kurse;
        private System.Windows.Forms.Button Graduierungsliste;
        private System.Windows.Forms.Button TrainingsteilnahmeKurse;
        private System.Windows.Forms.Button TrainingsteilnahmeMonate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}