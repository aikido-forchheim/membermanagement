using System;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeMember : AxisType
    {
        public AxisTypeMember()
        {
            P_ActiveElementsOnly = true;
            P_MaxDbId = Globals.DatabaseWrapper.MaxMitgliedsNr();
            P_MinDbId = Globals.DatabaseWrapper.MinMitgliedsNr();
    }

    public override string MouseCellEvent(DateTime datStart, DateTime datEnd, int idMember, bool action)
            => action
               ? ReportTrainingsParticipation.Show(new ReportWeekVsCourses(datStart, datEnd, idMember))
               : $"Klicken für Details zu Mitglied\n" + GetDescription(idMember);

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => Globals.DatabaseWrapper.m_mitglieder.FindIndex(t => tn.MitgliedID == t.Id);

        public override string GetDescription(int idMember)
        {
            Mitglied mitglied = Globals.DatabaseWrapper.MitgliedFromId(idMember);
            return $"{mitglied.Vorname} {mitglied.Nachname} MitgliedNr. {mitglied.Id}";
        }
    }
}
