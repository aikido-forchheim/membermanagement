using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisMembers : VerticalAxis
    {
        public VerticalAxisMembers(DatabaseWrapper db, TrainingParticipationMatrix tpMatrix) 
            : base(db, tpMatrix)
        {
            m_iNrOfColsOnLeftSide  = 3;   // 3 columns for Mitglieder
            m_activeRowsOnly = true;
        }

        public override int GetNrOfSrcElements()
            => m_db.MaxMitgliedsNr() + 1;     // One additional row for pseudo member with Id = -1

        public override int GetIndexFromTrainingsParticipation( TrainingsTeilnahme tn )
            => tn.MitgliedID - 1;    // db ids start with 1, array indices with 0

        public override void FillRowHeaderCols( DataGridView dgv )
        {
            dgv.Columns[0].HeaderText = "Vorname";
            dgv.Columns[1].HeaderText = "Nachname";
            dgv.Columns[2].HeaderText = "Nr";

            int iDgvRow = 0;
            dgv.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            m_tpMatrix.ForAllRows
            (
                action: iRow =>
                {
                    Mitglied mitglied = m_db.MitgliedFromId(m_tpMatrix.GetRowId(iRow));
                    dgv[0, iDgvRow].Value = mitglied.Nachname;
                    dgv[1, iDgvRow].Value = mitglied.Vorname;
                    dgv[2, iDgvRow].Value = mitglied.Id;
                    ++iDgvRow;
                },
                activeRowsOnly: m_activeRowsOnly
            );
        }

        public override void FillRowSumCols(DataGridView dgv)
        {
            FillRowSumCols(dgv, activeRowsOnly: m_activeRowsOnly);
        }
    }
}
