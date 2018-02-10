using System;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    class HorizontalAxisTrainings : HorizontalAxis
    {
        private DateTime m_datStart;
        private DateTime m_datEnd;
        private int      m_idKurs;

        public HorizontalAxisTrainings
        (
            DateTime datStart, 
            DateTime datEnd, 
            int idKurs
        )
        {
            m_datStart = datStart;
            m_datEnd   = datEnd;
            m_idKurs   = idKurs;
            ActiveElementsOnly = true;
        }

        public override int NrOfSrcElements 
            => Globals.DatabaseWrapper.MaxTrainingNr();

        public override int GetIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => tn.TrainingID - 1;   // db ids start with 1, array indices with 0

        public override void FillHeaderCells(DataGridView dgv, TrainingParticipationModel m_tpModel, int iStartIndex)
        {
            dgv.Columns[0].HeaderText = "Monat\nTag";
            FillMainHeaderCells(dgv, m_tpModel, iStartIndex);
            dgv.Columns[dgv.ColumnCount - 1].HeaderText = "\nSumme";
        }

        protected override void FillHeaderCell(DataGridView dgv, int iDgvCol, int iCol)
        {
            Training training = Globals.DatabaseWrapper.TrainingFromId(iCol);
            dgv.Columns[iDgvCol].HeaderText = $"{training.Termin:MM}\n{training.Termin:dd}";
            dgv.Columns[iDgvCol].SortMode = DataGridViewColumnSortMode.NotSortable;
        }
    }
}
