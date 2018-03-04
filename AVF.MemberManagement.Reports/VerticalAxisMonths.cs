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

        protected override void FillAdditionalKeyHeaderCells(DataGridView dgv)
            => dgv.Columns[1].HeaderText = "Monat";

        protected override void FillAdditionalKeyCells(DataGridView dgv, int iDgvRow, int id)
            => dgv[1, iDgvRow].Value = AxisTypeMonth.GetDesc(id, ' ', m_datStart);
    }
}
