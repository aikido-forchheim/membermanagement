using System;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract partial class ReportTrainingsParticipation : ReportBase
    {
        protected VerticalAxis               m_yAxis   { get; private set; }
        protected HorizontalAxis             m_xAxis   { get; private set; }
        protected TrainingParticipationModel m_tpModel { get; private set; }

        protected DateTime m_datStart { get; private set; }
        protected DateTime m_datEnd   { get; private set; }

        public ReportTrainingsParticipation()
        {
            InitializeComponent();
        }

        protected void CreateModel
        (
            DateTime datStart,
            DateTime datEnd,
            HorizontalAxis xAxis, 
            VerticalAxis yAxis,
            Func<TrainingsTeilnahme, bool> filter
        )
        {
            m_datStart = datStart;
            m_datEnd = datEnd;
            m_xAxis = xAxis;
            m_yAxis = yAxis;
            m_xAxis.P_startIndex = m_yAxis.P_NrOfKeyColumns;
            List<TrainingsTeilnahme> tpList         = Globals.DatabaseWrapper.TrainingsTeilnahme(datStart, datEnd);
            List<TrainingsTeilnahme> tpListFiltered = Globals.DatabaseWrapper.Filter(tpList, filter);

            m_tpModel = new TrainingParticipationModel( tpListFiltered, m_xAxis, m_yAxis );
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
                        activeColsOnly: m_xAxis.P_ActiveElementsOnly
                    );
                    ++iDgvRow;
                },
                activeRowsOnly: m_yAxis.P_ActiveElementsOnly
            );
        }

        private void ReportFormSize()      // define dimensions of DataGridView
        {
            m_dataGridView.RowCount = m_yAxis.GetNrOfDgvRows(m_tpModel);
            m_dataGridView.ColumnCount = m_yAxis.P_NrOfKeyColumns;
            if (!m_xAxis.P_Hide)
                m_dataGridView.ColumnCount += m_xAxis.GetNrOfDgvCols(m_tpModel) + 1; // + 1 for summary column
        }           

        protected override void ReportFormPopulate()    // Fill cells of DataGridView
        {
            m_xAxis.Initialize();
            ReportFormSize();
            m_xAxis.FillMainHeaderCells(m_dataGridView, m_tpModel);
            m_dataGridView.Columns[m_dataGridView.ColumnCount - 1].HeaderText = "\nSumme";
            m_yAxis.FillKeyHeaderCells(m_dataGridView);
            m_yAxis.FillMainKeyCells(m_dataGridView, m_tpModel);
            if ( ! m_xAxis.P_Hide )
            {
                m_yAxis.FillSumCells(m_dataGridView, m_tpModel);
                FillMainArea();
            }
        }

        protected override string MouseCellEvent(int row, int col, bool action)
        {
            if (RowIsHeader(row))
            {
                if (ColIsInMainArea(col))
                {
                    return m_xAxis.P_AxisType.MouseCellEvent(m_datStart, m_datEnd, m_xAxis.GetColKey(col), action);
                }
                else // column header, key or summary columns
                {
                    return $"Klicken um nach {m_dataGridView.Columns[col].HeaderText} zu sortieren";
                }
            }
            else // data row
            {
                int idRowKey = (int)m_dataGridView[m_yAxis.P_KeyColumn, row].Value;

                if (ColIsInMainArea(col))
                {
                    return MouseMainDataAreaCellEvent(m_datStart, m_datEnd, idRowKey, m_xAxis.GetColKey(col), action);
                }
                else // key or summary 
                {
                    return m_yAxis.P_AxisType.MouseCellEvent(m_datStart, m_datEnd, idRowKey, action);
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
