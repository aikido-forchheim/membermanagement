using System;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract class AxisType : IAxis
    {
        // properties 

        public int P_MaxDbId { get; protected set; }
        public int P_MinDbId { get; protected set; }

        protected ReportDescriptor P_reportDescriptor { get; private set; }

        public List<string> HeaderStrings { get; set; }

        public bool P_ActiveElementsOnly { get; protected set; } = false;

        // constructor

        public AxisType(ReportDescriptor desc)
            => P_reportDescriptor = desc;

        // member functions

        public int DatabaseIdRange()
            => P_MaxDbId - P_MinDbId + 1;

        public int GetModelIndexFromTrainingsParticipation(TrainingsTeilnahme tn)
            => GetModelIndexFromId(GetIdFromTrainingsParticipation(tn));

        public string GetFullDesc(int id, string separator)
            => String.Join(separator, GetDescription(id));

        // abstract member functions

        public abstract string MouseAxisEvent(int id, bool action);
        public abstract int    GetModelIndexFromId(int id);
        public abstract int    GetIdFromModelIndex(int id);
        public abstract int    GetIdFromTrainingsParticipation(TrainingsTeilnahme tn);
        public abstract List<string> GetDescription(int id);
    }
}
