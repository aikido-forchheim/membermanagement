using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisMembers : VerticalAxis
    {
        public VerticalAxisMembers()
            => P_NrOfKeyColumns = 3;   // 3 columns for Mitglieder

        public override void FillKeyHeaderCells(DataGridView dgv)
        {
            base.FillKeyHeaderCells(dgv);
            dgv.Columns[1].HeaderText = "Vorname";
            dgv.Columns[2].HeaderText = "Nachname";
        }

        public override int FillMainKeyCell(DataGridView dgv, int iDgvRow, int iModelRow, AxisType axisType)
        {
            Mitglied mitglied = Globals.DatabaseWrapper.m_mitglieder[iModelRow];
            dgv[1, iDgvRow].Value = mitglied.Vorname;
            dgv[2, iDgvRow].Value = mitglied.Nachname;
            return base.FillMainKeyCell(dgv, iDgvRow, iModelRow, axisType);
        }
    }
}
