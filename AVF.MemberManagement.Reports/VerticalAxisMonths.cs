using System;
using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisMonths : VerticalAxis
    {
        DateTime m_datStart;

        public VerticalAxisMonths(DateTime datStart, DateTime datEnd)
        {
            P_NrOfKeyColumns = 1;
            m_datStart = datStart;
            P_AxisType = new AxisTypeMonth(datStart, datEnd);
        }

        public override void FillKeyHeaderCells(DataGridView dgv)
            => dgv.Columns[0].HeaderText = "Monat";

        public override int FillMainKeyCell(DataGridView dgv, int iDgvRow, int iModelRow)
        {
            int id = base.FillMainKeyCell(dgv, iDgvRow, iModelRow);
            dgv[0, iDgvRow].Value = AxisTypeMonth.GetDesc(id, ' ', m_datStart);
            return id;
        }
    }
}
