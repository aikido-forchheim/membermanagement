using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisMembers : VerticalAxis
    {
        public VerticalAxisMembers() 
        {
            NrOfKeyColumns  = 3;   // 3 columns for Mitglieder
            ActiveElementsOnly = true;
        }

        public override int MaxDatabaseId
            => Globals.DatabaseWrapper.MaxMitgliedsNr();

        public override int MinDatabaseId
            => Globals.DatabaseWrapper.MinMitgliedsNr();

        public override int ModelRange()
            => DatabaseIdRange();

        public override int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => Globals.DatabaseWrapper.m_mitglieder.FindIndex(t => tn.MitgliedID == t.Id);

        private int GetDbIndexFromModelIndex(int iModelIndex)
            => iModelIndex;

        public override void FillKeyCells( DataGridView dgv, TrainingParticipationModel tpModel)
        {
            dgv.Columns[0].HeaderText = "Vorname";
            dgv.Columns[1].HeaderText = "Nachname";
            dgv.Columns[2].HeaderText = "Nr";

            FillMainKeyCells(dgv, tpModel);
        }

        protected override void FillHeaderCell( TrainingParticipationModel tpModel, DataGridView dgv, int iDgvRow, int iModelRow)
        {
            int      dbIndex  = GetDbIndexFromModelIndex(iModelRow);
            Mitglied mitglied = Globals.DatabaseWrapper.m_mitglieder[dbIndex];
            dgv[0, iDgvRow].Value = mitglied.Vorname;
            dgv[1, iDgvRow].Value = mitglied.Nachname;
            dgv[2, iDgvRow].Value = mitglied.Id;
        }
    }
}
