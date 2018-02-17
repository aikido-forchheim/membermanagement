using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    public abstract class HorizontalAxis : Axis
    {
        public static int NrOfLeadingElements { get; protected set; }
        public static int NrOfTrailingElements { get; protected set; }

        public int GetNrOfAdditionalElements()
            => NrOfLeadingElements + NrOfTrailingElements;

        protected int[] m_DbIds;

        protected HorizontalAxis()
        {
            NrOfLeadingElements  = 0; // header not counted, is part of DataGridView
            NrOfTrailingElements = 1; // 1 row for column sum

            m_DbIds = new int[ModelRange()];
        }

        public int GetDbId(int iDgvCol)
            => m_DbIds[iDgvCol];

        protected abstract int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn);

        protected abstract int GetModelIndexFromId(int id);

        public override int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => GetModelIndexFromId(GetIdFromTrainingsParticipation(tn));

        public abstract int GetNrOfDgvCols(TrainingParticipationModel tpModel);

        protected abstract void FillHeaderCell(DataGridView dgv, int iDgvCol, int iModelCol);

        public abstract void FillHeaderCells(DataGridView dgv, TrainingParticipationModel tpModel);

        public void FillMainHeaderCells(DataGridView dgv, TrainingParticipationModel tpModel)
        {
            int iDgvCol = StartIndex;
            tpModel.ForAllCols
            (
                action: iModelCol => FillHeaderCell(dgv, iDgvCol++, iModelCol),
                activeColsOnly: ActiveElementsOnly
            );
        }

        public void FillSumCells(DataGridView dgv, TrainingParticipationModel tpModel)
        {
            int iDgvRow = dgv.RowCount - 1;  // one footer row

            dgv[StartIndex, iDgvRow].Value = "Summe";

            int iDgvCol = StartIndex;
            tpModel.ForAllCols
            (
                action: iModelCol => dgv[iDgvCol++, iDgvRow].Value = tpModel.GetColSum(iModelCol),
                activeColsOnly: ActiveElementsOnly
            );

            dgv[dgv.ColumnCount - 1, iDgvRow].Value = tpModel.GetSumSum();
        }
    }
}
