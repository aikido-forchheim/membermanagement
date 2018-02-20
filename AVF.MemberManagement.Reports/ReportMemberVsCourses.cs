﻿using System;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class ReportMemberVsCourses : ReportForm
    {
        public ReportMemberVsCourses( DateTime datStart, DateTime datEnd )
            : base(datStart, datEnd)
        {
            m_xAxis = new HorizontalAxisCourses( );
            m_yAxis = new VerticalAxisMembers( );

            m_xAxis.StartIndex = VerticalAxis.NrOfLeadingElements;
            m_yAxis.StartIndex = HorizontalAxis.NrOfLeadingElements;

            m_tpModel = new TrainingParticipationModel
            (
                datStart,
                datEnd,
                m_xAxis,
                m_yAxis,
                filter: tn => true
            );

            m_label1.Text = "Überblick Trainingsteilnahme";
            m_label2.Text = Globals.GetTimeRangeDescription(datStart, datEnd);

            m_dataGridView.CellMouseClick += new DataGridViewCellMouseEventHandler(CellMouseClick);
            m_dataGridView.CellMouseEnter += new DataGridViewCellEventHandler(CellMouseEnter);
            m_dataGridView.CellMouseLeave += new DataGridViewCellEventHandler(CellMouseLeave);
        }
        /*
                private void CellMouseClick(Object sender, DataGridViewCellMouseEventArgs e)
                {
                    ReportForm newForm = null;

                    if (ColIsKeyArea(e.ColumnIndex) || ColIsSummary(e.ColumnIndex))
                    {
                        if (RowIsHeader(e.RowIndex) || RowIsFooter(e.RowIndex))
                        {
                        }
                        else
                        {
                            int idMember = (int)m_dataGridView[2, e.RowIndex].Value;
                            newForm = new ReportWeekVsCourses(m_datStart, m_datEnd, idMember);
                        }
                    }
                    else // Main area column
                    {
                        int idKurs = m_xAxis.GetDbId(e.ColumnIndex - m_xAxis.StartIndex);
                        newForm = new ReportMemberVsTrainings(m_datStart, m_datEnd, idKurs);
                    }

                    if (newForm != null)
                        newForm.Show();
                }

                protected override string ToolTipText(DataGridViewCellEventArgs e)
                {
                    if (ColIsKeyArea(e.ColumnIndex) || ColIsSummary(e.ColumnIndex))
                    {
                        if (RowIsHeader(e.RowIndex) || RowIsFooter(e.RowIndex))
                        {
                        }
                        else // Main area row
                        {
                            int      idMember = (int)m_dataGridView[2, e.RowIndex].Value;
                            Mitglied member   = Globals.DatabaseWrapper.MitgliedFromId(idMember);
                            return $"Klicken für Details zu Mitglied\n {member.Vorname} {member.Nachname}";
                        }
                    }
                    else // Main area column
                    {
                        if (RowIsHeader(e.RowIndex) || RowIsFooter(e.RowIndex))
                        {
                            return $"Klicken für Details zu diesem Kurs";
                        }
                        else // Main area column
                        {
                            int idMember = (int)m_dataGridView[2, e.RowIndex].Value;
                            Mitglied member = Globals.DatabaseWrapper.MitgliedFromId(idMember);
                            return $"Klicken für Details zur Teilnahme von\n {member.Vorname} {member.Nachname} an diesem Kurs";
                        }
                    }
                    return String.Empty;
                }
        */

        private void CellMouseClick(Object sender, DataGridViewCellMouseEventArgs e)
        {
            MouseCellEvent(e.RowIndex, e.ColumnIndex, action:true);
        }

        protected override string ToolTipText(DataGridViewCellEventArgs e)
        {
            return MouseCellEvent(e.RowIndex, e.ColumnIndex, action: false);
        }

        private string MouseCellEvent( int row, int col, bool action )
        {
            ReportForm newForm  = null;
            int        idKurs   = -1;
            int        idMember = -1;
            Mitglied   member   = null;
            string     helpText = String.Empty;

            if (RowIsInMainArea(row))
            {
                idMember = (int)m_dataGridView[2, row].Value;
                member = Globals.DatabaseWrapper.MitgliedFromId(idMember);
            }

            if (ColIsInMainArea(col))
            {
                idKurs = m_xAxis.GetDbId(col - m_xAxis.StartIndex);
            }

            if (RowIsInMainArea(row))
            {
                if ( ColIsInMainArea( col ) )
                {
                    if (action)
                        newForm = new ReportMemberVsTrainings(m_datStart, m_datEnd, idKurs);
                    else
                        helpText = $"Klicken für Details zur Teilnahme von\n {member.Vorname} {member.Nachname} an diesem Kurs";
                }
                else
                {
                    if (action)
                        newForm = new ReportWeekVsCourses(m_datStart, m_datEnd, idMember);
                    else
                        helpText = $"Klicken für Details zu Mitglied\n {member.Vorname} {member.Nachname}";
                }
            }
            else if ( ColIsInMainArea( col ) )
            {
                if (action)
                    newForm = new ReportMemberVsTrainings(m_datStart, m_datEnd, idKurs);
                else
                    helpText = $"Klicken für Details zu diesem Kurs";
            }

            if (newForm != null)
                newForm.Show();

            return helpText;
        }
    }
}
