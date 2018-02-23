using System;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    public abstract class HorizontalAxis : Axis
    {
        public int P_startIndex { get; set; } // horizontal axis starts at this column

        public bool P_Hide { get; protected set; } = false;

        protected int[] m_DbIds;

        protected HorizontalAxis(int additionalElements = 0) : base(additionalElements) { }

        public void Initialize( )
            => m_DbIds = new int[ModelRange()];

        public int GetColKey(int iDgvCol)
            => m_DbIds[iDgvCol - P_startIndex];

        protected virtual int GetModelIndexFromId(int id)
            => 0;

        public override int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => GetModelIndexFromId(P_AxisType.GetIdFromTrainingsParticipation(tn));

        public virtual int GetNrOfDgvCols(TrainingParticipationModel tpModel)
            => 1;

        protected virtual void FillHeaderCell(DataGridView dgv, int iDgvCol, int iModelCol) { }

        public void FillMainHeaderCells(DataGridView dgv, TrainingParticipationModel tpModel)
        {
            int iDgvCol = P_startIndex;
            tpModel.ForAllCols
            (
                action: iModelCol => FillHeaderCell(dgv, iDgvCol++, iModelCol),
                activeColsOnly: P_ActiveElementsOnly
            );
        }
    }
}
