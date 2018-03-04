using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisMembers : VerticalAxis
    {
        public VerticalAxisMembers()
            => P_NrOfKeyColumns = 3;   // 3 columns for Mitglieder

        protected override void FillAdditionalKeyHeaderCells(DataGridView dgv)
        {
            dgv.Columns[1].HeaderText = "Vorname";
            dgv.Columns[2].HeaderText = "Nachname";
        }

        protected override void FillAdditionalKeyCells(DataGridView dgv, int iDgvRow, int id)
        {
            Mitglied mitglied = Globals.DatabaseWrapper.MitgliedFromId(id);
            dgv[1, iDgvRow].Value = mitglied.Vorname;
            dgv[2, iDgvRow].Value = mitglied.Nachname;
        }
    }
}
