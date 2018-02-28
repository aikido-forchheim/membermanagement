using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisMembers : VerticalAxis
    {
        public VerticalAxisMembers()
        {
            P_NrOfKeyColumns = 3;   // 3 columns for Mitglieder
            new AxisTypeMember();
        }

        public override void FillKeyHeaderCells(DataGridView dgv)
        {
            dgv.Columns[0].HeaderText = "Vorname";
            dgv.Columns[1].HeaderText = "Nachname";
            dgv.Columns[2].HeaderText = "Nr";
        }

        public override int FillMainKeyCell(DataGridView dgv, int iDgvRow, int iModelRow, AxisType axisType)
        {
            Mitglied mitglied = Globals.DatabaseWrapper.m_mitglieder[iModelRow];
            dgv[0, iDgvRow].Value = mitglied.Vorname;
            dgv[1, iDgvRow].Value = mitglied.Nachname;
            dgv[2, iDgvRow].Value = mitglied.Id;
            return base.FillMainKeyCell(dgv, iDgvRow, iModelRow, axisType);
        }
    }
}
