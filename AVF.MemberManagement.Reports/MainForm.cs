using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public partial class MainForm : Form
    {
        private DatabaseWrapper m_dbWrapper;
        private DataGridView    m_dataGridView;
        private ReportBase      m_report;

        public MainForm(DatabaseWrapper db)
        {
            InitializeComponent();
            Load += new EventHandler(MainFormLoad);
            m_dbWrapper = db;
        }

        private String BuildToolTip(int iRow, int iCol)
        {
            StringBuilder messageBoxCS = new StringBuilder();
            messageBoxCS.AppendFormat("{0} = {1}", "ColumnIndex", iCol);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "RowIndex", iRow);
            messageBoxCS.AppendLine();
            if (iRow == -1)
            {
                if ((3 <= iCol) && (iCol <= 13))
                {
                    int kursId = iCol - m_report.m_iNrOfColsOnLeftSide;
                    messageBoxCS.AppendFormat($"KursId = {kursId}");
                }
            }
            else if (iRow == m_dataGridView.RowCount - 1)
            {
                if ((3 <= iCol) && (iCol <= 13))
                {
                    int kursId = iCol - m_report.m_iNrOfColsOnLeftSide;
                    messageBoxCS.AppendFormat($"KursId = {kursId} Summe");
                }
            }
            else
            {
                if (iCol < 3)
                {
                    messageBoxCS.AppendFormat($"Selection: Mitglied {m_dataGridView[2, iRow].Value}");
                }
                else if (iCol == 14)
                {
                    messageBoxCS.AppendFormat($"Selection: Mitglied {m_dataGridView[2, iRow].Value} Summe");
                }
                else  // main area
                {
                    if (iCol == 3)
                    {
                        messageBoxCS.AppendFormat("Kurs = Lehrgäng und Sondertrainings");
                    }
                    else
                    {
                        int kursId = iCol - m_report.m_iNrOfColsOnLeftSide;
                        Kurs kurs = m_dbWrapper.KursFromId(kursId);
                        messageBoxCS.AppendFormat($"Kurs = { m_dbWrapper.WeekDay(kurs.WochentagID).Substring(0, 2) }{ kurs.Zeit:hh}:{ kurs.Zeit:mm}");
                    }
                    messageBoxCS.AppendLine();
                    messageBoxCS.AppendFormat("{0} = {1}", "Mitglied", m_dataGridView[2, iRow].Value);
                }
            }

            messageBoxCS.AppendLine();
            return messageBoxCS.ToString();
        }

        private void CellMouseClick(Object sender, DataGridViewCellMouseEventArgs e)
        {
            MessageBox.Show(BuildToolTip(e.RowIndex, e.ColumnIndex), "CellMouseClick Event");
        }

        private void MainFormLoad(System.Object sender, EventArgs e)
        {
            int iJahr = 2017;

            DateTime datStart = new DateTime(iJahr, 1, 1);
            DateTime datEnd = new DateTime(iJahr, 12, 31);

            m_dataGridView = new DataGridView
            {
                MultiSelect = false,
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                Dock = DockStyle.Fill
            };

            m_dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
            m_dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

            m_dataGridView.Location = new Point(8, 8);
            m_dataGridView.Size = new Size(500, 250);

            Controls.Add(m_dataGridView);
            m_dataGridView.CellMouseClick += new DataGridViewCellMouseEventHandler(CellMouseClick);

            Size = new Size(1000, 500);

            // m_report = new ReportMemberVsCourse(m_dbWrapper, datStart, datEnd);
            // m_report.PopulateGridView(m_dataGridView, activeCellsOnly: true);
            // m_report = new ReportWeekVsCourse(m_dbWrapper, datStart, datEnd, 144);
            // m_report.PopulateGridView(m_dataGridView, activeCellsOnly:false);
            m_report = new ReportMemberVsTraining(m_dbWrapper, datStart, datEnd, 7);
            m_report.PopulateGridView(m_dataGridView, activeCellsOnly:true);
        }
    }
}