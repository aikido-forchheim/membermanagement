using System;
using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisMonths : VerticalAxis
    {
        private DateTime m_datStart;

        public VerticalAxisMonths(DateTime datStart, DateTime datEnd)
        {
            P_NrOfKeyColumns = 2;
            m_datStart = datStart;
        }

        public override void FillKeyHeaderCells(DataGridView dgv)
        {
            base.FillKeyHeaderCells(dgv);
            dgv.Columns[1].HeaderText = "Monat";
        }

        public override int FillMainKeyCell(DataGridView dgv, int iDgvRow, int iModelRow, AxisType axisType)
        {
            int id = base.FillMainKeyCell(dgv, iDgvRow, iModelRow, axisType);
            dgv[1, iDgvRow].Value = AxisTypeMonth.GetDesc(id, ' ', m_datStart);
            return id;
        }
    }
}
