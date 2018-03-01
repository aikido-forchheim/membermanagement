using System;
using System.Diagnostics;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public partial class ReportTrainingsParticipation : ReportBase
    {
        protected VerticalAxis               m_yAxis     { get; private set; }
        protected HorizontalAxis             m_xAxis     { get; private set; }
        protected AxisType                   m_xAxisType { get; private set; }
        protected AxisType                   m_yAxisType { get; private set; }
        protected TrainingParticipationModel m_tpModel   { get; private set; }

        protected DateTime m_datStart { get; private set; }
        protected DateTime m_datEnd   { get; private set; }

        private bool P_Hide { get; set; } = false;

        public ReportTrainingsParticipation()
            =>InitializeComponent(); // creates DataGridView ...

        protected void CreateModel
        (
            bool bHide,
            DateTime datStart,
            DateTime datEnd,
            AxisType xAxisType,
            AxisType yAxisType,
            Func<TrainingsTeilnahme, bool> filter
        )
        {
            P_Hide = bHide;

            m_datStart = datStart;
            m_datEnd = datEnd;

            m_xAxis = yAxisType.GetHorizontalAxis();
            m_yAxis = yAxisType.GetVerticalAxis();

            m_xAxisType = xAxisType;
            m_yAxisType = yAxisType;

            m_xAxis.P_startIndex = m_yAxis.P_NrOfKeyColumns;
            m_yAxis.P_startIndex = m_xAxis.P_NrOfKeyColumns;

            List<TrainingsTeilnahme> tpList         = Globals.DatabaseWrapper.TrainingsTeilnahme(datStart, datEnd);
            List<TrainingsTeilnahme> tpListFiltered = Globals.DatabaseWrapper.Filter(tpList, filter);

            m_tpModel = new TrainingParticipationModel( tpListFiltered, m_xAxisType, m_yAxisType );
            m_labelZeitraum.Text = Globals.GetTimeRangeDescription(datStart, datEnd);
        }

        private void FillMainArea()
        {
            int iDgvRow = 0;
            m_tpModel.ForAllRows
            (
                action: iModelRow =>
                {
                    int iDgvCol = m_yAxis.P_NrOfKeyColumns;
                    m_tpModel.ForAllCols
                    (
                        action: iModelCol => m_dataGridView[iDgvCol++, iDgvRow].Value = FormatMatrixElement(m_tpModel.GetCell(iModelRow, iModelCol)),
                        activeColsOnly: m_xAxisType.P_ActiveElementsOnly
                    );
                    ++iDgvRow;
                },
                activeRowsOnly: m_yAxisType.P_ActiveElementsOnly
            );
        }

        private void ReportFormSize()      // define dimensions of DataGridView
        {
            m_dataGridView.RowCount    = m_yAxisType.P_ActiveElementsOnly ? m_tpModel.GetNrOfActiveRows() : m_yAxisType.DatabaseIdRange();
            m_dataGridView.ColumnCount = m_yAxis.P_NrOfKeyColumns;
            if (!P_Hide)
                m_dataGridView.ColumnCount += m_xAxisType.P_ActiveElementsOnly ? m_tpModel.GetNrOfActiveCols() : m_xAxisType.DatabaseIdRange() + 1; // + 1 for summary column
        }           

        protected override void ReportFormPopulate()    // Fill cells of DataGridView
        {
            m_xAxis.Initialize( m_xAxisType.DatabaseIdRange() );
            m_yAxis.Initialize( m_yAxisType.DatabaseIdRange() );
            ReportFormSize();

            m_yAxis.FillKeyHeaderCells(m_dataGridView);

            int iDgvRow = 0;
            m_tpModel.ForAllRows
            (
                action: iModelRow => m_yAxis.FillMainKeyCell(m_dataGridView, iDgvRow++, iModelRow, m_yAxisType),
                activeRowsOnly: m_yAxisType.P_ActiveElementsOnly
            );

            if (!P_Hide)
            {
                int iDgvCol = m_xAxis.P_startIndex;
                m_tpModel.ForAllCols
                (
                    action: iModelCol => m_xAxis.FillMainKeyCell(m_dataGridView, iDgvCol++, iModelCol, m_xAxisType),
                    activeColsOnly: m_xAxisType.P_ActiveElementsOnly
                );

                m_dataGridView.Columns[m_dataGridView.ColumnCount - 1].HeaderText = "\nSumme";

                iDgvRow = 0;
                m_tpModel.ForAllRows
                (
                    iModelRow => m_dataGridView[m_dataGridView.ColumnCount - 1, iDgvRow++].Value = m_tpModel.GetRowSum(iModelRow),
                    m_yAxisType.P_ActiveElementsOnly
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
                    return m_xAxisType.MouseCellEvent(m_datStart, m_datEnd, m_xAxis.GetDbIdFromDgvIndex(col), action);
                }
                else // column header, key or summary columns
                {
                    return $"Klicken um nach {m_dataGridView.Columns[col].HeaderText} zu sortieren";
                }
            }
            else // data row
            {
                if (ColIsInMainArea(col))
                {
                    return MouseMainDataAreaCellEvent(m_datStart, m_datEnd, m_yAxis.GetDbIdFromDgvIndex(row), m_xAxis.GetDbIdFromDgvIndex(col), action);
                }
                else // key or summary 
                {
                    return m_yAxisType.MouseCellEvent(m_datStart, m_datEnd, m_yAxis.GetDbIdFromDgvIndex(row), action);
                }
            }
        }

        public static string Show( ReportTrainingsParticipation report )
        {
            report.Show();
            return String.Empty;
        }

        protected virtual string MouseMainDataAreaCellEvent(DateTime datStart, DateTime datEnd, int row, int col, bool action)
            => String.Empty;

        private bool ColIsKeyArea(int iCol)
            => iCol < m_yAxis.P_NrOfKeyColumns;

        private bool ColIsSummary(int iCol)
            => iCol == m_dataGridView.ColumnCount - 1;

        private bool ColIsInMainArea(int iCol)
            => !(ColIsKeyArea(iCol) || ColIsSummary(iCol));

        protected virtual string FormatMatrixElement(int iValue)
            => (iValue > 0) ? $"{ iValue, -3 }" : "   ";
    }
}
