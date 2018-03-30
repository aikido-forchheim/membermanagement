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
               ? ReportMain.P_formMain.NewPanel(typeof(AxisTypeCourse), typeof(AxisTypeMonth), P_reportDescriptor.P_timeRange, idMember: idMember)
               : $"Klicken für Details zu Mitglied\n" + GetFullDesc(idMember);

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => tn.MitgliedID;

        public override string GetFullDesc(int idMember, char separator = ' ')
            => (idMember < 0) ? "Alle Mitglieder" : GetDescription(idMember, 1) + separator + GetDescription(idMember, 2);

        public override string GetDescription(int idMember, int iNr)
        {
            if (idMember > 0)
            {
                Mitglied mitglied = Globals.DatabaseWrapper.MitgliedFromId(idMember);

                switch (iNr)
                {
                    case 0:
                        return $"MitgliedNr. {mitglied.Id}";

                    case 1:
                        return $"{mitglied.Vorname}";

                    case 2:
                        return $"{mitglied.Nachname}";

                    default:
                        Debug.Assert(false);
                        break;
                }
            }
            return String.Empty;
        }
    }
}
