using System;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract class AxisType : IAxis
    {
        // properties 

        protected int P_MaxDbId { get; set; }
        protected int P_MinDbId { get; set; }

        protected DateTime P_datStart { get; set; }
        protected DateTime P_datEnd { get; set; }

        public List<string> HeaderStrings { get; set; }

        public bool P_ActiveElementsOnly { get; protected set; } = false;

        // constructor

        public AxisType(DateTime datStart, DateTime datEnd)
        {
            P_datStart = datStart;
            P_datEnd = datEnd;
        }

        // member functions

        public int DatabaseIdRange()
            => P_MaxDbId - P_MinDbId + 1;

        public int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => GetModelIndexFromId(GetIdFromTrainingsParticipation(tn));

        public virtual string GetFullDesc(int id, char separator = ' ')
                => GetDescription(id);

        // abstract member functions

        public abstract string MouseAxisEvent(int id, bool action);
        public abstract int    GetModelIndexFromId(int id);
        public abstract int    GetIdFromModelIndex(int id);
        public abstract int    GetIdFromTrainingsParticipation(TrainingsTeilnahme tn);
        public abstract string GetDescription(int id, int iNr = 1);
    }
}
