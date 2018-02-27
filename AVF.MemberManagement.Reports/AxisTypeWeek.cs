using System;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class AxisTypeWeek : AxisType
    {
        DateTime m_datStart;
        DateTime m_datEnd;

        private int NrOfWeeks(DateTime datStart, DateTime datEnd)
        {
            return (datEnd - datStart).Days / 7;
        }

        public AxisTypeWeek(DateTime datStart, DateTime datEnd)
        { 
            P_ActiveElementsOnly = false;
            m_datStart = datStart;
            m_datEnd = datEnd;
            P_MinDbId = 0;
            P_MaxDbId = (datEnd - datStart).Days / 7;
        }
        
        public override string MouseCellEvent(DateTime datStart, DateTime datEnd, int id, bool action)
            => action
               ? String.Empty
               : $"Klicken für Details zu Trainings in dieser Woche (Noch nicht implementiert)";

        public override int GetIdFromTrainingsParticipation(TrainingsTeilnahme tn)
            => Globals.GetWeekOfYear(Globals.DatabaseWrapper.TerminFromTrainingId(tn.TrainingID));

        public override string GetDescription(int id, char separator)
        {
            DateTime date = m_datStart.AddDays(7 * id);
            int year = date.Year;
            int week = Globals.GetWeekOfYear( date );
            return $"{week + 1}{separator}{year}";
        }
    }
}
