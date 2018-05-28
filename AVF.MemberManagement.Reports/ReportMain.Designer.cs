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
            this.labelLadeDatenbank = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.schließenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.berichteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trainingsteilnahmeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.monateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kurseToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.jahreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kurseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graduierungslisteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mitgliederbeiträgeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MemberFees = new System.Windows.Forms.Button();
            this.panelLoadDb = new System.Windows.Forms.Panel();
            this.labelAnimation = new System.Windows.Forms.Label();
            this.buttonUndo = new System.Windows.Forms.Button();
            this.buttonRedo = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.panelLoadDb.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(88, 92);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(417, 51);
            this.progressBar1.Step = 6;
            this.progressBar1.TabIndex = 0;
            // 
            // labelLadeDatenbank
            // 
            this.labelLadeDatenbank.AutoSize = true;
            this.labelLadeDatenbank.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLadeDatenbank.Location = new System.Drawing.Point(82, 42);
            this.labelLadeDatenbank.Name = "labelLadeDatenbank";
            this.labelLadeDatenbank.Size = new System.Drawing.Size(224, 32);
            this.labelLadeDatenbank.TabIndex = 2;
            this.labelLadeDatenbank.Text = "Lade Datenbank";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem,
            this.berichteToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(10, 10);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(819, 33);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dateiToolStripMenuItem
            // 
            this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportierenToolStripMenuItem,
            this.schließenToolStripMenuItem,
            this.infoToolStripMenuItem});
            this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
            this.dateiToolStripMenuItem.Size = new System.Drawing.Size(65, 29);
            this.dateiToolStripMenuItem.Text = "Datei";
            // 
            // exportierenToolStripMenuItem
            // 
            this.exportierenToolStripMenuItem.Name = "exportierenToolStripMenuItem";
            this.exportierenToolStripMenuItem.Size = new System.Drawing.Size(185, 30);
            this.exportierenToolStripMenuItem.Text = "Exportieren";
            this.exportierenToolStripMenuItem.Click += new System.EventHandler(this.Export_Click);
            // 
            // schließenToolStripMenuItem
            // 
            this.schließenToolStripMenuItem.Name = "schließenToolStripMenuItem";
            this.schließenToolStripMenuItem.Size = new System.Drawing.Size(185, 30);
            this.schließenToolStripMenuItem.Text = "Schließen";
            this.schließenToolStripMenuItem.Click += new System.EventHandler(this.ApplicationExit_Click);
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(185, 30);
            this.infoToolStripMenuItem.Text = "Info";
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.Info_Click);
            // 
            // berichteToolStripMenuItem
            // 
            this.berichteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trainingsteilnahmeToolStripMenuItem,
            this.kurseToolStripMenuItem,
            this.graduierungslisteToolStripMenuItem,
            this.mitgliederbeiträgeToolStripMenuItem});
            this.berichteToolStripMenuItem.Name = "berichteToolStripMenuItem";
            this.berichteToolStripMenuItem.Size = new System.Drawing.Size(86, 29);
            this.berichteToolStripMenuItem.Text = "Berichte";
            // 
            // trainingsteilnahmeToolStripMenuItem
            // 
            this.trainingsteilnahmeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.monateToolStripMenuItem,
            this.kurseToolStripMenuItem1,
            this.jahreToolStripMenuItem});
            this.trainingsteilnahmeToolStripMenuItem.Name = "trainingsteilnahmeToolStripMenuItem";
            this.trainingsteilnahmeToolStripMenuItem.Size = new System.Drawing.Size(242, 30);
            this.trainingsteilnahmeToolStripMenuItem.Text = "Trainingsteilnahme";
            // 
            // monateToolStripMenuItem
            // 
            this.monateToolStripMenuItem.Name = "monateToolStripMenuItem";
            this.monateToolStripMenuItem.Size = new System.Drawing.Size(157, 30);
            this.monateToolStripMenuItem.Text = "Monate";
            this.monateToolStripMenuItem.Click += new System.EventHandler(this.Trainingsteilnahme_Monate_Click);
            // 
            // kurseToolStripMenuItem1
            // 
            this.kurseToolStripMenuItem1.Name = "kurseToolStripMenuItem1";
            this.kurseToolStripMenuItem1.Size = new System.Drawing.Size(157, 30);
            this.kurseToolStripMenuItem1.Text = "Kurse";
            this.kurseToolStripMenuItem1.Click += new System.EventHandler(this.Trainingsteilnahme_Kurse_Click);
            // 
            // jahreToolStripMenuItem
            // 
            this.jahreToolStripMenuItem.Name = "jahreToolStripMenuItem";
            this.jahreToolStripMenuItem.Size = new System.Drawing.Size(157, 30);
            this.jahreToolStripMenuItem.Text = "Jahre";
            this.jahreToolStripMenuItem.Click += new System.EventHandler(this.Trainingsteilnahme_Jahre_Click);
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
            // mitgliederbeiträgeToolStripMenuItem
            // 
            this.mitgliederbeiträgeToolStripMenuItem.Name = "mitgliederbeiträgeToolStripMenuItem";
            this.mitgliederbeiträgeToolStripMenuItem.Size = new System.Drawing.Size(242, 30);
            this.mitgliederbeiträgeToolStripMenuItem.Text = "Mitgliederbeiträge";
            this.mitgliederbeiträgeToolStripMenuItem.Click += new System.EventHandler(this.MemberFees_Click);
            // 
            // MemberFees
            // 
            this.MemberFees.Enabled = false;
            this.MemberFees.FlatAppearance.BorderSize = 0;
            this.MemberFees.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ControlLight;
            this.MemberFees.Location = new System.Drawing.Point(503, 33);
            this.MemberFees.Name = "MemberFees";
            this.MemberFees.Size = new System.Drawing.Size(171, 52);
            this.MemberFees.TabIndex = 9;
            this.MemberFees.Text = "Mitgliederbeiträge";
            this.MemberFees.UseVisualStyleBackColor = true;
            this.MemberFees.Click += new System.EventHandler(this.MemberFees_Click);
            // 
            // panelLoadDb
            // 
            this.panelLoadDb.Controls.Add(this.progressBar1);
            this.panelLoadDb.Controls.Add(this.labelLadeDatenbank);
            this.panelLoadDb.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelLoadDb.Location = new System.Drawing.Point(10, 72);
            this.panelLoadDb.Name = "panelLoadDb";
            this.panelLoadDb.Size = new System.Drawing.Size(819, 674);
            this.panelLoadDb.TabIndex = 11;
            // 
            // labelAnimation
            // 
            this.labelAnimation.Font = new System.Drawing.Font("Arial Narrow", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAnimation.Location = new System.Drawing.Point(269, 5);
            this.labelAnimation.Name = "labelAnimation";
            this.labelAnimation.Size = new System.Drawing.Size(224, 38);
            this.labelAnimation.TabIndex = 7;
            this.labelAnimation.Text = "................";
            this.labelAnimation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonUndo
            // 
            this.buttonUndo.AutoSize = true;
            this.buttonUndo.Enabled = false;
            this.buttonUndo.Image = global::AVF.MemberManagement.Reports.Properties.Resources.Undo_16x_32;
            this.buttonUndo.Location = new System.Drawing.Point(192, 14);
            this.buttonUndo.Name = "buttonUndo";
            this.buttonUndo.Size = new System.Drawing.Size(22, 23);
            this.buttonUndo.TabIndex = 8;
            this.buttonUndo.UseMnemonic = false;
            this.buttonUndo.UseVisualStyleBackColor = true;
            this.buttonUndo.Click += new System.EventHandler(this.Undo_Click);
            // 
            // buttonRedo
            // 
            this.buttonRedo.AutoSize = true;
            this.buttonRedo.Enabled = false;
            this.buttonRedo.Image = global::AVF.MemberManagement.Reports.Properties.Resources.Redo_16x_32;
            this.buttonRedo.Location = new System.Drawing.Point(220, 14);
            this.buttonRedo.Name = "buttonRedo";
            this.buttonRedo.Size = new System.Drawing.Size(26, 23);
            this.buttonRedo.TabIndex = 12;
            this.buttonRedo.UseVisualStyleBackColor = true;
            this.buttonRedo.Click += new System.EventHandler(this.Redo_Click);
            // 
            // ReportMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 756);
            this.Controls.Add(this.labelAnimation);
            this.Controls.Add(this.buttonRedo);
            this.Controls.Add(this.buttonUndo);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panelLoadDb);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::AVF.MemberManagement.Reports.Properties.Settings.Default, "WindowLocation", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.KeyPreview = true;
            this.Location = global::AVF.MemberManagement.Reports.Properties.Settings.Default.WindowLocation;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ReportMain";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = " AVF Mitgliederverwaltung";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelLoadDb.ResumeLayout(false);
            this.panelLoadDb.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label labelLadeDatenbank;
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
        private System.Windows.Forms.Button MemberFees;
        private System.Windows.Forms.ToolStripMenuItem mitgliederbeiträgeToolStripMenuItem;
        private System.Windows.Forms.Panel panelLoadDb;
        private System.Windows.Forms.Button buttonUndo;
        private System.Windows.Forms.Button buttonRedo;
        private System.Windows.Forms.Label labelAnimation;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jahreToolStripMenuItem;
    }
}