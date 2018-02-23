using System;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeMember : AxisType
    {
        public override string MouseCellEvent(DateTime datStart, DateTime datEnd, int idMember, bool action)
            => action
               ? ReportTrainingsParticipation.Show(new ReportWeekVsCourses(datStart, datEnd, idMember))
               : $"Klicken für Details zu Mitglied\n" + Globals.GetMemberDescription(idMember);

        public override int P_MaxDbId { get; } = Globals.DatabaseWrapper.MaxMitgliedsNr();

        public override int P_MinDbId { get; } = Globals.DatabaseWrapper.MinMitgliedsNr();

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => Globals.DatabaseWrapper.m_mitglieder.FindIndex(t => tn.MitgliedID == t.Id);
    }
}
