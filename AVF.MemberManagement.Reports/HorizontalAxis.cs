using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    public abstract class HorizontalAxis : Axis
    {
        protected HorizontalAxis()
        {
            NrOfLeadingElements  = 0; // header not counted, is part of DataGridView
            NrOfTrailingElements = 1; // 1 row for column sum
        }

        protected abstract int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn);

        protected abstract int GetModelIndexFromId(int id);

        public override int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => GetModelIndexFromId(GetIdFromTrainingsParticipation(tn));

        public abstract int GetNrOfDgvCols(TrainingParticipationModel tpModel);

        protected abstract void FillHeaderCell(DataGridView dgv, int iDgvCol, int iModelCol);

        public abstract void FillHeaderCells(DataGridView dgv, TrainingParticipationModel tpModel, int iDgvStartIndex);

        public void FillMainHeaderCells(DataGridView dgv, TrainingParticipationModel tpModel, int iDgvStartIndex)
        {
            int iDgvCol = iDgvStartIndex;
            tpModel.ForAllCols
            (
                action: iModelCol => FillHeaderCell(dgv, iDgvCol++, iModelCol),
                activeColsOnly: ActiveElementsOnly
            );
        }

        public void FillSumCells(DataGridView dgv, TrainingParticipationModel tpModel, int iNrOfColsOnLeftSide)
        {
            int iDgvRow = dgv.RowCount - 1;  // one footer row

            dgv[iNrOfColsOnLeftSide - 1, iDgvRow].Value = "Summe";

            int iDgvCol = iNrOfColsOnLeftSide;
            tpModel.ForAllCols
            (
                action: iModelCol => dgv[iDgvCol++, iDgvRow].Value = tpModel.GetColSum(iModelCol),
                activeColsOnly: ActiveElementsOnly
            );

            dgv[dgv.ColumnCount - 1, iDgvRow].Value = tpModel.GetSumSum();
        }
    }
}
