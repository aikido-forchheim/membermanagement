using System;
using System.Windows.Forms;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisWeeks : VerticalAxis
    {
        private DateTime m_datStart;

        public VerticalAxisWeeks(DateTime datStart, DateTime datEnd)
        {
            P_NrOfKeyColumns = 2;
            m_datStart = datStart;
        }

        protected override void FillAdditionalKeyHeaderCells(DataGridView dgv)
            => dgv.Columns[1].HeaderText = "KW";

        protected override void FillAdditionalKeyCells(DataGridView dgv, int iDgvRow, int id)
            => dgv[1, iDgvRow].Value = AxisTypeWeek.GetDesc(id, '/', m_datStart);
    }
}
