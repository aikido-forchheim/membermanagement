﻿using System;
using System.Windows.Forms;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract partial class ReportTrainingsParticipation : ReportBase
    {
        protected VerticalAxis               P_yAxis     { get; private set; }
        protected HorizontalAxis             P_xAxis     { get; private set; }
        protected AxisType                   P_xAxisType { get; private set; }
        protected AxisType                   P_yAxisType { get; private set; }
        protected TrainingParticipationModel P_tpModel   { get; private set; }

        protected AxisTypeMember P_axisTypeMember { get; private set; }

        protected ReportDescriptor m_reportDescriptor;

        public ReportTrainingsParticipation
        (
            TimeRange timeRange = Globals.ALL_YEARS, 
            int idMember = Globals.ALL_MEMBERS, 
            int idCourse = Globals.ALL_COURSES, 
            int idTraining = Globals.ALL_TRAININGS,
            int idMonth = Globals.ALL_MONTHS
        )
        {
            m_reportDescriptor = new ReportDescriptor(timeRange, idMember, idCourse, idTraining, idMonth);

            InitializeReportTrainingsParticipation(); // creates DataGridView ...
            P_labelReportName.Text = "Trainingsteilnahme";
            P_labelZeitraum.Text = Globals.GetTimeRangeDescription(m_reportDescriptor.P_timeRange);
            P_axisTypeMember = new AxisTypeMember(m_reportDescriptor);
            P_dataGridView.Sorted += new EventHandler(delegate (object s, EventArgs e) { Sorted(s, e); });
            P_yearSelector.Minimum = Globals.DatabaseWrapper.GetStartValidData().Year;
            P_yearSelector.Maximum = DateTime.Now.Year;
            P_yearSelector.Value = P_yearSelector.Maximum - 1;
        }

        protected void CreateModel
        (
            AxisType xAxisType,
            AxisType yAxisType
        )
        {
            P_xAxis = new HorizontalAxis();
            P_yAxis = new VerticalAxis();

            P_xAxisType = xAxisType;
            P_yAxisType = yAxisType;

            P_xAxis.P_startIndex = yAxisType.HeaderStrings.Count + 1; ;
            P_yAxis.P_startIndex = 0;

            List<TrainingsTeilnahme> tpList = Globals.DatabaseWrapper.P_trainingsTeilnahme;

            if (m_reportDescriptor.P_timeRange != Globals.ALL_YEARS)
                tpList = Globals.DatabaseWrapper.Filter(tpList, tn => m_reportDescriptor.P_timeRange.Includes(Globals.DatabaseWrapper.TerminFromTrainingId(tn.TrainingID)));

            Globals.DatabaseWrapper.TrainingsTeilnahme(m_reportDescriptor.P_timeRange);

            if (m_reportDescriptor.P_idMember != Globals.ALL_MEMBERS)
                tpList = Globals.DatabaseWrapper.Filter(tpList, tn => m_reportDescriptor.P_idMember == tn.MitgliedID);

            if (m_reportDescriptor.P_idCourse != Globals.ALL_COURSES)
                tpList = Globals.DatabaseWrapper.Filter(tpList, tn => m_reportDescriptor.P_idCourse == Globals.DatabaseWrapper.KursIdFromTrainingId(tn.TrainingID));

            if (m_reportDescriptor.P_idTraining != Globals.ALL_TRAININGS)
                tpList = Globals.DatabaseWrapper.Filter(tpList, tn => tn.TrainingID == m_reportDescriptor.P_idTraining);

            P_tpModel = new TrainingParticipationModel(tpList, P_xAxisType, P_yAxisType );
        }

        private void FillMainArea()
        {
            int iDgvRow = 0;
            P_tpModel.ForAllRows
            (
                action: iModelRow =>
                {
                    int iDgvCol = P_xAxis.P_startIndex;
                    P_tpModel.ForAllCols
                    (
                        action: iModelCol => P_dataGridView[iDgvCol++, iDgvRow].Value = FormatMatrixElement(P_tpModel.GetCell(iModelRow, iModelCol)),
                        activeColsOnly: P_xAxisType.P_ActiveElementsOnly
                    );
                    ++iDgvRow;
                },
                activeRowsOnly: P_yAxisType.P_ActiveElementsOnly
            );
        }

        private void ReportFormSize()      // define dimensions of DataGridView
        {
            P_dataGridView.RowCount = (P_yAxisType.P_ActiveElementsOnly ? P_tpModel.GetNrOfActiveRows() : P_yAxisType.DatabaseIdRange()) + 1; // + 1 for summary row
            P_dataGridView.ColumnCount = P_yAxisType.HeaderStrings.Count + 1;
            if (P_xAxisType.P_MaxDbId > 0)
                P_dataGridView.ColumnCount += (P_xAxisType.P_ActiveElementsOnly ? P_tpModel.GetNrOfActiveCols() : P_xAxisType.DatabaseIdRange()) + 1; // + 1 for summary column
        }           

        protected override void ReportFormPopulate()    // Fill cells of DataGridView
        {
            P_xAxis.Initialize( P_xAxisType.DatabaseIdRange() );
            P_yAxis.Initialize( P_yAxisType.DatabaseIdRange() );
            ReportFormSize();

            P_labelMember.Text = P_axisTypeMember.GetFullDesc(m_reportDescriptor.P_idMember);
            P_labelMonat.Text  = new AxisTypeMonth(m_reportDescriptor).GetDescription(m_reportDescriptor.P_idMonth);
            if (m_reportDescriptor.P_idCourse != Globals.ALL_COURSES)
            {
                int idTrainer = Globals.DatabaseWrapper.KursFromId(m_reportDescriptor.P_idCourse).Trainer;
                if (idTrainer > 0)
                    P_labelTrainer.Text = $"Trainer: {P_axisTypeMember.GetFullDesc(idTrainer)}";
            }

            P_yAxis.FillKeyHeaderCells(P_dataGridView, P_yAxisType);

            int iDgvRow = 0;
            P_tpModel.ForAllRows
            (
                action: iModelRow => P_yAxis.FillMainKeyCell(P_dataGridView, iDgvRow++, iModelRow, P_yAxisType),
                activeRowsOnly: P_yAxisType.P_ActiveElementsOnly
            );

            if (P_xAxisType.P_MaxDbId > 0)
            {
                int iDgvCol = P_xAxis.P_startIndex;
                P_tpModel.ForAllCols
                (
                    action: iModelCol => P_xAxis.FillMainKeyCell(P_dataGridView, iDgvCol++, iModelCol, P_xAxisType),
                    activeColsOnly: P_xAxisType.P_ActiveElementsOnly
                );

                P_dataGridView.Columns[P_dataGridView.ColumnCount - 1].HeaderText = "\nSumme";

                iDgvRow = 0;
                P_tpModel.ForAllRows
                (
                    action: iModelRow => P_dataGridView[P_dataGridView.ColumnCount - 1, iDgvRow++].Value = P_tpModel.GetRowSum(iModelRow),
                    activeRowsOnly: P_yAxisType.P_ActiveElementsOnly
                );

                FillMainArea();
                FillSummaryRow();
            }
        }
 
        private void Sorted(object sender, EventArgs e)                        // After sorting, summary row may be in wrong place
        {
            for ( int row = 0; row < P_dataGridView.RowCount; row++ )
            {
                if (P_dataGridView.Rows[row].Cells[0].Value == null)           // Find summary row (it is the only row with empty cell[0]),
                {
                    P_dataGridView.Rows.Remove(P_dataGridView.Rows[row]);      // remove it
                    break;
                }
            }

            P_dataGridView.Rows.Insert(P_dataGridView.RowCount);               // and add new summary row at end if grid
            FillSummaryRow();
        }

        private void FillSummaryRow()
        {
            int sumSum    = 0;
            int rowNrSum  = P_dataGridView.RowCount - 1;
            int iDgvCol   = P_xAxis.P_startIndex;
            P_dataGridView.Rows[rowNrSum].Cells[iDgvCol-1].Value = "Summe";
            P_tpModel.ForAllCols
            (
                action: iModelCol =>
                {
                    int iColSum = 0;
                    P_tpModel.ForAllRows
                    (
                        action: iModelRow => iColSum += P_tpModel.GetCell(iModelRow, iModelCol),
                        activeRowsOnly: P_yAxisType.P_ActiveElementsOnly
                    );
                    P_dataGridView.Rows[rowNrSum].Cells[iDgvCol].Value = iColSum;
                    sumSum += iColSum;
                    ++iDgvCol;
                },
                activeColsOnly: P_xAxisType.P_ActiveElementsOnly
            );
            P_dataGridView.Rows[rowNrSum].Cells[iDgvCol].Value = sumSum;
        }

        protected override string MouseCellEvent(int row, int col, bool action)
        {
            if (IsHeaderRow(row))
            {
                if (ColIsInMainArea(col))
                {
                    return P_xAxisType.MouseAxisEvent(P_xAxis.GetDbIdFromDgvIndex(col), action);
                }
                else // column header, key or summary columns
                {
                    return $"Klicken um nach {P_dataGridView.Columns[col].HeaderText} zu sortieren";
                }
            }
            else if (IsSummaryRow(row))
            {
                return String.Empty;  // TODO: add some action
            }
            else // data row
            {
                if (ColIsInMainArea(col))
                {
                    return MouseMainDataAreaCellEvent(m_reportDescriptor.P_timeRange, P_yAxis.GetDbIdFromDgvIndex(P_dataGridView, row), P_xAxis.GetDbIdFromDgvIndex(col), action);
                }
                else // key or summary 
                {
                    return P_yAxisType.MouseAxisEvent(P_yAxis.GetDbIdFromDgvIndex(P_dataGridView, row), action);
                }
            }
        }

        private void YearSelectionChanged(object sender, EventArgs e)
        {
            int iYear = (int)((NumericUpDown)sender).Value;
            if ((P_yearSelector.Minimum <= iYear) && (iYear <= P_yearSelector.Maximum))
            {
/*
                m_iYearStart = new DateTime((int)P_yearSelector.Value, 1, 1);
                m_iYearEnd = new DateTime((int)P_yearSelector.Value, 12, 31);
                ReportBase newReport = m_UndoRedo.P_reportActual;
                if (newReport != null)
                {
                    System.Type reportType = newReport.GetType();
                    //                    ReportBase report = new (newReport.GetType());
                    m_UndoRedo.SwitchTo(newReport); // (new DateTime(m_iYear, 1, 1), new DateTime(m_iYear, 12, 31)));
                }
*/
            }
        }

        protected virtual string MouseMainDataAreaCellEvent(TimeRange timeRange, int row, int col, bool action)
            => String.Empty;

        private bool ColIsKeyArea(int iCol)
            => iCol < P_yAxisType.HeaderStrings.Count + 1;

        private bool ColIsSummary(int iCol)
            => iCol == P_dataGridView.ColumnCount - 1;

        private bool ColIsInMainArea(int iCol)
            => !(ColIsKeyArea(iCol) || ColIsSummary(iCol));

        protected virtual string FormatMatrixElement(int iValue)
            => (iValue > 0) ? $"{ iValue, -3 }" : "   ";
    }
}
