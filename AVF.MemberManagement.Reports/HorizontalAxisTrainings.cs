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
            DatabaseWrapper db, 
            TrainingParticipationMatrix tpMatrix, 
            DateTime datStart, 
            DateTime datEnd, 
            int idKurs
        )
            : base(db, tpMatrix)
        {
            m_datStart = datStart;
            m_datEnd   = datEnd;
            m_idKurs   = idKurs;
            m_activeColsOnly = true;
        }

        public override int GetNrOfSrcElements()
            => m_db.MaxTrainingNr();

        public override int GetIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => tn.TrainingID - 1;   // db ids start with 1, array indices with 0

        public override void FillHeaderRows(DataGridView dgv, int iNrOfColsOnLeftSide)
        {
            dgv.Columns[0].HeaderText = "Monat\nTag";
            int iCol = iNrOfColsOnLeftSide;
            foreach (var training in m_db.TrainingsInPeriod(m_idKurs, m_datStart, m_datEnd))
            {
                dgv.Columns[iCol++].HeaderText = $"{training.Termin:MM}\n{training.Termin:dd}";
            }
            dgv.Columns[dgv.ColumnCount - 1].HeaderText = "\nSumme";
        }
    }
}
