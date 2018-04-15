using System;
using System.Diagnostics;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeMember : AxisType
    {
        public AxisTypeMember(ReportDescriptor desc)
             : base(desc)
        {
            P_ActiveElementsOnly = true;
            P_MaxDbId = Globals.DatabaseWrapper.MaxMitgliedsNr();
            P_MinDbId = Globals.DatabaseWrapper.MinMitgliedsNr();
            HeaderStrings = new List<string> { "Vorname", "Nachname" };
        }

        public override int GetModelIndexFromId(int id)
            => Globals.DatabaseWrapper.P_mitglieder.FindIndex(x => id == x.Id);

        public override int GetIdFromModelIndex(int iModelIndex)
            => Globals.DatabaseWrapper.P_mitglieder[iModelIndex].Id;

        public override string MouseAxisEvent(int idMember, bool action)
            => action
               ? ReportMain.P_formMain.NewTrainingsParticipationPanel
                 (
                     P_reportDescriptor,
                     typeof(AxisTypeCourse), 
                     typeof(AxisTypeMonth), 
                     idMember: idMember
                 )
               : $"Klicken für Details zu Mitglied\n" + GetFullDesc(idMember, " ");

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => tn.MitgliedID;

        public override List<string> GetDescription(int idMember)
            => GetDesc(idMember);

        public static List<string> GetDesc(int idMember)
        {
            Debug.Assert(idMember >= 0);
            List<string> list = new List<string>();
            Mitglied mitglied = Globals.DatabaseWrapper.MitgliedFromId(idMember);
            list.Add($"{mitglied.Vorname}");
            list.Add($"{mitglied.Nachname}");
            return list;
        }

        public static string GetFullName(Mitglied member)
            => $"{member.Vorname} {member.Nachname}";
    }
}
