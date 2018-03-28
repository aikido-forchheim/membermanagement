﻿using System;
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

        protected TimeRange P_timeRange { get; private set; }

        public List<string> HeaderStrings { get; set; }

        public bool P_ActiveElementsOnly { get; protected set; } = false;

        // constructor

        public AxisType(TimeRange timeRange)
            => P_timeRange = timeRange;

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
