using System;
using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisWeeks : VerticalAxis
    {
        DateTime m_datStart;

        public VerticalAxisWeeks(DateTime datStart, DateTime datEnd)
        {
            P_NrOfKeyColumns = 1;
            m_datStart = datStart;
        }

        public override void FillKeyHeaderCells(DataGridView dgv)
            => dgv.Columns[0].HeaderText = "KW";

        public override int FillMainKeyCell(DataGridView dgv, int iDgvRow, int iModelRow, AxisType axisType)
        {
            int id = base.FillMainKeyCell(dgv, iDgvRow, iModelRow, axisType);
            dgv[0, iDgvRow].Value = AxisTypeWeek.GetDesc(id, '/', m_datStart);
            return id;
        }
    }
}
