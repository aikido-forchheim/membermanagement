using System;
using System.Drawing;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberFees : ReportBase
    {
        private decimal m_halfYearSum = 0;

        public ReportMemberFees()
        {
            InitializeComponent(); // creates DataGridView ...

            m_dataGridView.Columns.Add("surname",      "Name");
            m_dataGridView.Columns.Add("firstName",    "Vorname");
            m_dataGridView.Columns.Add("hemberId",     "lfd.\nNr.");
            m_dataGridView.Columns.Add("memberClass",  "Bei-\ntrags-\nklasse");
            m_dataGridView.Columns.Add("stdFee",       "voller\nBeitrag\np.a.");
            m_dataGridView.Columns.Add("familymember", "Familien-\nmitglied");
            m_dataGridView.Columns.Add("effectiveFee", "Beitrag\np.a.");
            m_dataGridView.Columns.Add("halfYearFee",  "Lastschrift");

            m_dataGridView.RowHeadersVisible = false;
            m_dataGridView.AllowUserToAddRows = false;
            m_dataGridView.EnableHeadersVisualStyles = false;

            foreach (DataGridViewColumn cols in m_dataGridView.Columns)
                cols.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        protected override void ReportFormPopulate()
        {
            foreach (Mitglied member in Globals.DatabaseWrapper.CurrentMembers())
            {
                decimal yearlyFee = MemberFees.GetMemberFee(member);
                if ( yearlyFee > 0 )
                {
                    m_dataGridView.Rows.Add
                    (
                        member.Nachname,
                        member.Vorname,
                        member.Id,
                        Globals.DatabaseWrapper.BK_Text(member),
                        Globals.DatabaseWrapper.BK(member).Beitrag,
                        member.Familienmitglied,
                        yearlyFee,
                        yearlyFee / 2
                    );
                    m_halfYearSum += yearlyFee / 2;
                }
            }
        }

        protected override string MouseCellEvent(int row, int col, bool action)
        {
            if (row >= 0)
            {
                return String.Empty;
            }
            else
            {
                return String.Empty;
            }
        }
    }
}