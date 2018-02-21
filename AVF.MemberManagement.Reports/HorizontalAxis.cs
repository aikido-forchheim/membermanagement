using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    public abstract class HorizontalAxis : Axis
    {
        public int StartIndex { get; set; } // horizontal axis starts at this column

        protected int[] m_DbIds;

        protected HorizontalAxis( )
            =>  m_DbIds = new int[ModelRange()];

        public int GetDbId(int iDgvCol)
            => m_DbIds[iDgvCol - StartIndex];

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
    }
}
