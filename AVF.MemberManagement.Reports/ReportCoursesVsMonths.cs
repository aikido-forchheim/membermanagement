﻿using System;

namespace AVF.MemberManagement.Reports
{
    class ReportCoursesVsMonths : ReportTrainingsParticipation
    {
        public ReportCoursesVsMonths(DateTime datStart, DateTime datEnd, int idMember)
        {
            CreateModel
            (
                bHide: false,
                datStart, datEnd,
                new AxisTypeMonth(datStart, datEnd),
                new AxisTypeCourse(),
                filter: tn => (idMember == -1) ? true : (idMember == tn.MitgliedID)
            );

            m_labelReportName.Text = "Überblick Kursteilnahme";
            if (idMember != -1)
                m_Info0.Text = AxisTypeMember.GetDesc(idMember, ' ');
        }
    }
}
