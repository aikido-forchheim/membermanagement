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
            this.m_label1 = new System.Windows.Forms.Label();
            this.m_label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.m_dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // m_dataGridView
            // 
            this.m_dataGridView.AllowUserToAddRows = false;
            this.m_dataGridView.AllowUserToDeleteRows = false;
            this.m_dataGridView.Dock = System.Windows.Forms.DockStyle.Right;
            this.m_dataGridView.EnableHeadersVisualStyles = false;
            this.m_dataGridView.Location = new System.Drawing.Point(-1, 0);
            this.m_dataGridView.MultiSelect = false;
            this.m_dataGridView.Name = "m_dataGridView";
            this.m_dataGridView.RowHeadersVisible = false;
            this.m_dataGridView.RowTemplate.Height = 28;
            this.m_dataGridView.Size = new System.Drawing.Size(1345, 712);
            this.m_dataGridView.TabIndex = 0;
            this.m_dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_dataGridView_CellContentClick);
            // 
            // m_label1
            // 
            this.m_label1.AutoSize = true;
            this.m_label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_label1.Location = new System.Drawing.Point(66, 44);
            this.m_label1.Name = "m_label1";
            this.m_label1.Size = new System.Drawing.Size(111, 37);
            this.m_label1.TabIndex = 1;
            this.m_label1.Text = "Label1";
            this.m_label1.Click += new System.EventHandler(this.m_labelFormName_Click);
            // 
            // m_label2
            // 
            this.m_label2.AutoSize = true;
            this.m_label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_label2.Location = new System.Drawing.Point(66, 131);
            this.m_label2.Name = "m_label2";
            this.m_label2.Size = new System.Drawing.Size(113, 37);
            this.m_label2.TabIndex = 2;
            this.m_label2.Text = "Label2";
            this.m_label2.Click += new System.EventHandler(this.m_labelTimeRange_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(66, 217);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 37);
            this.label3.TabIndex = 3;
            this.label3.Text = "Label3";
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1344, 712);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_label2);
            this.Controls.Add(this.m_label1);
            this.Controls.Add(this.m_dataGridView);
            this.Name = "ReportForm";
            this.Text = "AVF Mitgliederdatenbank";
            ((System.ComponentModel.ISupportInitialize)(this.m_dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected DataGridView m_dataGridView;
        protected Label m_label1;
        protected Label m_label2;
        protected Label label3;
    }
}