using System;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberFees : ReportBase
    {
        private decimal P_halfYearSum { get; set; } = 0;

        public ReportMemberFees()
        {
            InitializeReportBase(); // creates DataGridView ...

            P_dataGridView.Columns.Add("surname",      "Name");
            P_dataGridView.Columns.Add("firstName",    "Vorname");
            P_dataGridView.Columns.Add("hemberId",     "lfd.\nNr.");
            P_dataGridView.Columns.Add("memberClass",  "Bei-\ntrags-\nklasse");
            P_dataGridView.Columns.Add("stdFee",       "voller\nBeitrag\np.a.");
            P_dataGridView.Columns.Add("familymember", "Familien-\nmitglied");
            P_dataGridView.Columns.Add("effectiveFee", "Beitrag\np.a.");
            P_dataGridView.Columns.Add("halfYearFee",  "Lastschrift");

            P_dataGridView.RowHeadersVisible = false;
            P_dataGridView.AllowUserToAddRows = false;
            P_dataGridView.EnableHeadersVisualStyles = false;

            foreach (DataGridViewColumn cols in P_dataGridView.Columns)
                cols.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            ReportFormPopulate();
        }

        protected override void ReportFormPopulate()
        {
            foreach (Mitglied member in Globals.DatabaseWrapper.CurrentMembers())
            {
                decimal yearlyFee = MemberFees.GetMemberFee(member);
                if ( yearlyFee > 0 )
                {
                    P_dataGridView.Rows.Add
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
                    P_halfYearSum += yearlyFee / 2;
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