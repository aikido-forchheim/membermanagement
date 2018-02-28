﻿using System;

namespace AVF.MemberManagement.Reports
{
    class ReportMonthsVsCourses : ReportTrainingsParticipation
    {
        public ReportMonthsVsCourses(DateTime datStart, DateTime datEnd, int idMember)
        {
            CreateModel
            (
                bHide: false,
                datStart, datEnd,
                new AxisTypeCourse(),
                new AxisTypeMonth(datStart, datEnd),
                new VerticalAxisMonths(datStart, datEnd),
                filter: tn => idMember == tn.MitgliedID
            );

            m_labelReportName.Text = "Trainingsteilnahme ";
            m_Info0.Text  = AxisTypeMember.GetDesc(idMember, ' ');
        }
    }
}
