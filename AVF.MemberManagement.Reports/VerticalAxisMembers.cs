using System;
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
            KeyColumn = 2;
            ActiveElementsOnly = true;
        }

        public override int MaxDatabaseId
            => Globals.DatabaseWrapper.MaxMitgliedsNr();

        public override int MinDatabaseId
            => Globals.DatabaseWrapper.MinMitgliedsNr();

        public override int GetNrOfDgvRows(TrainingParticipationModel tpModel)
            => tpModel.GetNrOfActiveRows();

        protected override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => Globals.DatabaseWrapper.m_mitglieder.FindIndex(t => tn.MitgliedID == t.Id);

        public override int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => GetIdFromTrainingsParticipation( tn );

        private int GetDbIndexFromModelIndex(int iModelIndex)
            => iModelIndex;

        public override void FillKeyHeaderCells(DataGridView dgv)
        {
            dgv.Columns[0].HeaderText = "Vorname";
            dgv.Columns[1].HeaderText = "Nachname";
            dgv.Columns[2].HeaderText = "Nr";
        }

        protected override void FillMainKeyCell( TrainingParticipationModel tpModel, DataGridView dgv, int iDgvRow, int iModelRow)
        {
            int      dbIndex  = GetDbIndexFromModelIndex(iModelRow);
            Mitglied mitglied = Globals.DatabaseWrapper.m_mitglieder[dbIndex];
            dgv[0, iDgvRow].Value = mitglied.Vorname;
            dgv[1, iDgvRow].Value = mitglied.Nachname;
            dgv[2, iDgvRow].Value = mitglied.Id;
        }

        public override string MouseKeyCellEvent(DateTime datStart, DateTime datEnd, int idMember, bool action)
        {
           return action
               ? ReportTrainingsParticipation.Show(new ReportWeekVsCourses(datStart, datEnd, idMember))
               : $"Klicken für Details zu Mitglied\n" + Globals.GetMemberDescription(idMember);
        }
    }
}
