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
            this.components = new System.ComponentModel.Container();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.Kurse = new System.Windows.Forms.Button();
            this.Graduierungsliste = new System.Windows.Forms.Button();
            this.TrainingsteilnahmeKurse = new System.Windows.Forms.Button();
            this.TrainingsteilnahmeMonate = new System.Windows.Forms.Button();
            this.labelLadeDatenbank = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.schließenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.berichteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trainingsteilnahmeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kurseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graduierungslisteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.monateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kurseToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
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
            // labelLadeDatenbank
            // 
            this.labelLadeDatenbank.AutoSize = true;
            this.labelLadeDatenbank.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLadeDatenbank.Location = new System.Drawing.Point(187, 338);
            this.labelLadeDatenbank.Name = "labelLadeDatenbank";
            this.labelLadeDatenbank.Size = new System.Drawing.Size(224, 32);
            this.labelLadeDatenbank.TabIndex = 2;
            this.labelLadeDatenbank.Text = "Lade Datenbank";
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
            // toolTip1
            // 
            this.toolTip1.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem,
            this.berichteToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(839, 33);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dateiToolStripMenuItem
            // 
            this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportierenToolStripMenuItem,
            this.schließenToolStripMenuItem});
            this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
            this.dateiToolStripMenuItem.Size = new System.Drawing.Size(65, 29);
            this.dateiToolStripMenuItem.Text = "Datei";
            // 
            // exportierenToolStripMenuItem
            // 
            this.exportierenToolStripMenuItem.Name = "exportierenToolStripMenuItem";
            this.exportierenToolStripMenuItem.Size = new System.Drawing.Size(210, 30);
            this.exportierenToolStripMenuItem.Text = "Exportieren";
            // 
            // schließenToolStripMenuItem
            // 
            this.schließenToolStripMenuItem.Name = "schließenToolStripMenuItem";
            this.schließenToolStripMenuItem.Size = new System.Drawing.Size(210, 30);
            this.schließenToolStripMenuItem.Text = "Schließen";
            this.schließenToolStripMenuItem.Click += new System.EventHandler(this.ApplicationExit_Click);
            // 
            // berichteToolStripMenuItem
            // 
            this.berichteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trainingsteilnahmeToolStripMenuItem,
            this.kurseToolStripMenuItem,
            this.graduierungslisteToolStripMenuItem});
            this.berichteToolStripMenuItem.Name = "berichteToolStripMenuItem";
            this.berichteToolStripMenuItem.Size = new System.Drawing.Size(86, 29);
            this.berichteToolStripMenuItem.Text = "Berichte";
            // 
            // trainingsteilnahmeToolStripMenuItem
            // 
            this.trainingsteilnahmeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.monateToolStripMenuItem,
            this.kurseToolStripMenuItem1});
            this.trainingsteilnahmeToolStripMenuItem.Name = "trainingsteilnahmeToolStripMenuItem";
            this.trainingsteilnahmeToolStripMenuItem.Size = new System.Drawing.Size(242, 30);
            this.trainingsteilnahmeToolStripMenuItem.Text = "Trainingsteilnahme";
            // 
            // kurseToolStripMenuItem
            // 
            this.kurseToolStripMenuItem.Name = "kurseToolStripMenuItem";
            this.kurseToolStripMenuItem.Size = new System.Drawing.Size(242, 30);
            this.kurseToolStripMenuItem.Text = "Kurse";
            this.kurseToolStripMenuItem.Click += new System.EventHandler(this.Kurse_Click);
            // 
            // graduierungslisteToolStripMenuItem
            // 
            this.graduierungslisteToolStripMenuItem.Name = "graduierungslisteToolStripMenuItem";
            this.graduierungslisteToolStripMenuItem.Size = new System.Drawing.Size(242, 30);
            this.graduierungslisteToolStripMenuItem.Text = "Graduierungsliste";
            this.graduierungslisteToolStripMenuItem.Click += new System.EventHandler(this.Gradierungsliste_Click);
            // 
            // monateToolStripMenuItem
            // 
            this.monateToolStripMenuItem.Name = "monateToolStripMenuItem";
            this.monateToolStripMenuItem.Size = new System.Drawing.Size(210, 30);
            this.monateToolStripMenuItem.Text = "Monate";
            this.monateToolStripMenuItem.Click += new System.EventHandler(this.Trainingsteilnahme_Monate_Click);
            // 
            // kurseToolStripMenuItem1
            // 
            this.kurseToolStripMenuItem1.Name = "kurseToolStripMenuItem1";
            this.kurseToolStripMenuItem1.Size = new System.Drawing.Size(210, 30);
            this.kurseToolStripMenuItem1.Text = "Kurse";
            this.kurseToolStripMenuItem1.Click += new System.EventHandler(this.Trainingsteilnahme_Kurse_Click);
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
            this.Controls.Add(this.labelLadeDatenbank);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ReportMain";
            this.Text = "ReportMain";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button Kurse;
        private System.Windows.Forms.Button Graduierungsliste;
        private System.Windows.Forms.Button TrainingsteilnahmeKurse;
        private System.Windows.Forms.Button TrainingsteilnahmeMonate;
        private System.Windows.Forms.Label labelLadeDatenbank;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportierenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem schließenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem berichteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trainingsteilnahmeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem monateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kurseToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem kurseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem graduierungslisteToolStripMenuItem;
    }
}