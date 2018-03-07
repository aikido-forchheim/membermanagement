using System;
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

        protected DateTime P_datStart { get; private set; }
        protected DateTime P_datEnd   { get; private set; }

        private bool P_Hide { get; set; } = false;

        public ReportTrainingsParticipation(DateTime datStart, DateTime datEnd)
        {
            P_datStart = datStart;
            P_datEnd = datEnd;

            InitializeReportTrainingsParticipation(); // creates DataGridView ...
            P_labelReportName.Text = "Trainingsteilnahme";
            P_labelZeitraum.Text = Globals.GetTimeRangeDescription(P_datStart, P_datEnd);
            P_axisTypeMember = new AxisTypeMember(P_datStart, P_datEnd);
        }

        protected void CreateModel
        (
            bool bHide,
            AxisType xAxisType,
            AxisType yAxisType,
            Func<TrainingsTeilnahme, bool> filter
        )
        {
            P_Hide = bHide;

            P_xAxis = new HorizontalAxis();
            P_yAxis = new VerticalAxis();

            P_xAxisType = xAxisType;
            P_yAxisType = yAxisType;

            P_xAxis.P_startIndex = yAxisType.HeaderStrings.Count + 1; ;
            P_yAxis.P_startIndex = 0;

            List<TrainingsTeilnahme> tpList         = Globals.DatabaseWrapper.TrainingsTeilnahme(P_datStart, P_datEnd);
            List<TrainingsTeilnahme> tpListFiltered = Globals.DatabaseWrapper.Filter(tpList, filter);

            P_tpModel = new TrainingParticipationModel( tpListFiltered, P_xAxisType, P_yAxisType );
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
            P_dataGridView.RowCount    = P_yAxisType.P_ActiveElementsOnly ? P_tpModel.GetNrOfActiveRows() : P_yAxisType.DatabaseIdRange();
            P_dataGridView.ColumnCount = P_yAxisType.HeaderStrings.Count + 1;
            if (!P_Hide)
                P_dataGridView.ColumnCount += (P_xAxisType.P_ActiveElementsOnly ? P_tpModel.GetNrOfActiveCols() : P_xAxisType.DatabaseIdRange()) + 1; // + 1 for summary column
        }           

        protected override void ReportFormPopulate()    // Fill cells of DataGridView
        {
            P_xAxis.Initialize( P_xAxisType.DatabaseIdRange() );
            P_yAxis.Initialize( P_yAxisType.DatabaseIdRange() );
            ReportFormSize();

            P_yAxis.FillKeyHeaderCells(P_dataGridView, P_yAxisType);

            int iDgvRow = 0;
            P_tpModel.ForAllRows
            (
                action: iModelRow => P_yAxis.FillMainKeyCell(P_dataGridView, iDgvRow++, iModelRow, P_yAxisType),
                activeRowsOnly: P_yAxisType.P_ActiveElementsOnly
            );

            if (!P_Hide)
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
            }
        }

        protected override string MouseCellEvent(int row, int col, bool action)
        {
            if (RowIsHeader(row))
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
            else // data row
            {
                if (ColIsInMainArea(col))
                {
                    return MouseMainDataAreaCellEvent(P_datStart, P_datEnd, P_yAxis.GetDbIdFromDgvIndex(P_dataGridView, row), P_xAxis.GetDbIdFromDgvIndex(col), action);
                }
                else // key or summary 
                {
                    return P_yAxisType.MouseAxisEvent(P_yAxis.GetDbIdFromDgvIndex(P_dataGridView, row), action);
                }
            }
        }

        protected virtual string MouseMainDataAreaCellEvent(DateTime datStart, DateTime datEnd, int row, int col, bool action)
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
