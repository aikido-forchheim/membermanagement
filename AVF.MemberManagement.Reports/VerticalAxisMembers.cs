using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class VerticalAxisMembers : VerticalAxis
    {
        public VerticalAxisMembers() 
        {
            NrOfLeadingElements  = 3;   // 3 columns for Mitglieder
            ActiveElementsOnly = true;
        }

        public override int NrOfSrcElements 
            => Globals.DatabaseWrapper.MaxMitgliedsNr() + 1;     // One additional row for pseudo member with Id = -1

        public override int GetIndexFromTrainingsParticipation( TrainingsTeilnahme tn )
            => tn.MitgliedID - 1;    // db ids start with 1, array indices with 0

        public override void FillHeaderCells( DataGridView dgv, TrainingParticipationModel tpModel, int iStartIndex)
        {
            dgv.Columns[0].HeaderText = "Vorname";
            dgv.Columns[1].HeaderText = "Nachname";
            dgv.Columns[2].HeaderText = "Nr";

            dgv.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            FillMainHeaderCells(dgv, tpModel, iStartIndex);
        }

        protected override void FillHeaderCell( TrainingParticipationModel tpModel, DataGridView dgv, int iDgvRow, int iRow)
        {
            Mitglied mitglied = Globals.DatabaseWrapper.MitgliedFromId(tpModel.GetRowId(iRow));
            dgv[0, iDgvRow].Value = mitglied.Nachname;
            dgv[1, iDgvRow].Value = mitglied.Vorname;
            dgv[2, iDgvRow].Value = mitglied.Id;
        }
    }
}
