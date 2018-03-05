using System;
using System.Diagnostics;
using System.Collections.Generic;
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
            HeaderStrings = new List<string> { "Vorname", "Nachname" };
        }

        public override int GetModelIndexFromId(int id)
            => Globals.DatabaseWrapper.m_mitglieder.FindIndex(x => id == x.Id);

        public override int GetIdFromModelIndex(int iModelIndex)
            => Globals.DatabaseWrapper.m_mitglieder[iModelIndex].Id;

        public override string MouseAxisEvent(DateTime datStart, DateTime datEnd, int idMember, bool action)
            => action
               ? ReportMain.SwitchToPanel(new ReportMonthsVsCourses(datStart, datEnd, idMember))
               : $"Klicken für Details zu Mitglied\n" + GetFullDesc(idMember);

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => tn.MitgliedID;

        public override string GetDescription(int idMember, int iNr)
            => GetDesc(idMember, iNr);

        public static string GetFullDesc(int idMember)
            => GetDesc(idMember, 1) + " " + GetDesc(idMember, 2);

        public static string GetDesc(int idMember, int iNr)
        {
            Mitglied mitglied = Globals.DatabaseWrapper.MitgliedFromId(idMember);

            switch ( iNr )
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
            return string.Empty;
        }
    }
}
