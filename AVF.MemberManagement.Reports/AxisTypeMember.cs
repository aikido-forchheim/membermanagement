using System;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.StandardLibrary.Interfaces;
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

        public override int GetModelIndexFromId(int id)
            => Globals.DatabaseWrapper.m_mitglieder.FindIndex(x => id == x.Id);

        public override int GetIdFromModelIndex(int iModelIndex)
            => Globals.DatabaseWrapper.m_mitglieder[iModelIndex].Id;

        public override string MouseCellEvent(DateTime datStart, DateTime datEnd, int idMember, bool action)
            => action
               ? ReportTrainingsParticipation.Show(new ReportMonthsVsCourses(datStart, datEnd, idMember))
               : $"Klicken für Details zu Mitglied\n" + GetDescription(idMember, ' ');

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => tn.MitgliedID;

        public override string GetDescription(int idMember, char separator)
            => GetDesc(idMember, separator);

        public static string GetDesc(int idMember, char separator)
        {
            Mitglied mitglied = Globals.DatabaseWrapper.MitgliedFromId(idMember);
            return $"{mitglied.Vorname} {mitglied.Nachname} MitgliedNr. {mitglied.Id}";
        }
    }
}
