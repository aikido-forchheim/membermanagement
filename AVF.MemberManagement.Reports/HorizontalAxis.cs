using System;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    public abstract class HorizontalAxis : Axis
    {
        public int StartIndex { get; set; } // horizontal axis starts at this column

        public bool Hide { get; protected set; } = false;

        protected int[] m_DbIds;

        protected HorizontalAxis( )
            =>  m_DbIds = new int[ModelRange()];

        public int GetColKey(int iDgvCol)
            => m_DbIds[iDgvCol - StartIndex];

        protected virtual int GetModelIndexFromId(int id)
            => 0;

        public override int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => GetModelIndexFromId(GetIdFromTrainingsParticipation(tn));

        public virtual int GetNrOfDgvCols(TrainingParticipationModel tpModel)
            => 1;

        public virtual string MouseHeaderCellEvent(DateTime datStart, DateTime datEnd, int id, bool action)
            => String.Empty;

        protected virtual void FillHeaderCell(DataGridView dgv, int iDgvCol, int iModelCol) { }

        public virtual void FillHeaderCells(DataGridView dgv, TrainingParticipationModel tpModel) { }

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
