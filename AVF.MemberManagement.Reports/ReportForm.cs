using System;
using System.Windows.Forms;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public abstract partial class ReportForm : Form
    {
        protected VerticalAxis               m_yAxis   { get; private set; }
        protected HorizontalAxis             m_xAxis   { get; private set; }
        protected TrainingParticipationModel m_tpModel { get; private set; }

        protected DateTime m_datStart { get; private set; }
        protected DateTime m_datEnd   { get; private set; }

        public ReportForm()
        {
            InitializeComponent();

            m_dataGridView.CellMouseClick += new DataGridViewCellMouseEventHandler(CellMouseClick);
            m_dataGridView.CellMouseEnter += new DataGridViewCellEventHandler(CellMouseEnter);
            m_dataGridView.CellMouseLeave += new DataGridViewCellEventHandler(CellMouseLeave);

            Load += new EventHandler(ReportFormLoad);
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
            m_xAxis.StartIndex = VerticalAxis.NrOfKeyColumns;
            List<TrainingsTeilnahme> tpList         = Globals.DatabaseWrapper.TrainingsTeilnahme(datStart, datEnd);
            List<TrainingsTeilnahme> tpListFiltered = Globals.DatabaseWrapper.Filter(tpList, filter);

            m_tpModel = new TrainingParticipationModel( tpListFiltered, m_xAxis, m_yAxis );
        }

        protected abstract string MouseCellEvent(int row, int col, bool action);

        protected void CellMouseClick(Object sender, DataGridViewCellMouseEventArgs e)
        {
            MouseCellEvent(e.RowIndex, e.ColumnIndex, action: true);
        }

        protected virtual void ReportFormPopulate()
        {
            // define dimensions of DataGridView

            m_dataGridView.RowCount    = m_yAxis.GetNrOfDgvRows(m_tpModel);
            m_dataGridView.ColumnCount = m_xAxis.GetNrOfDgvCols(m_tpModel) + VerticalAxis.NrOfKeyColumns + 1; // + 1 for summary column

            // Fill cells of DataGridView

            m_xAxis.FillHeaderCells(m_dataGridView, m_tpModel);
            m_yAxis.FillKeyCells(m_dataGridView, m_tpModel);
            m_yAxis.FillSumCells(m_dataGridView, m_tpModel);

            // Fill main area of DataGridView

            int iDgvRow = 0;
            m_tpModel.ForAllRows
            (
                action: iModelRow =>
                {
                    int iDgvCol = VerticalAxis.NrOfKeyColumns;
                    m_tpModel.ForAllCols
                    (
                        action: iModelCol => m_dataGridView[iDgvCol++, iDgvRow].Value = FormatMatrixElement(m_tpModel.GetCell(iModelRow, iModelCol)),
                        activeColsOnly: m_xAxis.ActiveElementsOnly
                    );
                    ++iDgvRow;
                },
                activeRowsOnly: m_yAxis.ActiveElementsOnly
            );
        }

        private void ReportFormLoad(System.Object sender, EventArgs e)
        {
            ReportFormPopulate();

            m_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            m_dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            m_dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            Controls.Add(m_dataGridView);
            WindowState = FormWindowState.Maximized;
            // Size = new Size(1000, 500);
        }

        protected string ToolTipText(DataGridViewCellEventArgs e)
        {
            return MouseCellEvent(e.RowIndex, e.ColumnIndex, action: false);
        }

        protected void ToolTip(DataGridViewCellEventArgs e, bool showTip)
        {
            DataGridViewCell cell =
                RowIsHeader(e.RowIndex)
                ? m_dataGridView.Columns[e.ColumnIndex].HeaderCell
                : m_dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

            cell.ToolTipText = showTip ? ToolTipText(e) : String.Empty;
        }

        protected void CellMouseEnter(object sender, DataGridViewCellEventArgs e)
           => ToolTip(e, true);

        protected void CellMouseLeave(object sender, DataGridViewCellEventArgs e)
            => ToolTip(e, false);

        protected bool RowIsHeader(int iRow)
            => iRow < 0;

        protected bool RowIsInMainArea(int iRow)
            => ! RowIsHeader(iRow);

        protected bool ColIsKeyArea(int iCol)
            => iCol < VerticalAxis.NrOfKeyColumns;

        protected bool ColIsSummary(int iCol)
            => iCol == m_dataGridView.ColumnCount - 1;

        protected bool ColIsInMainArea(int iCol)
            => !(ColIsKeyArea(iCol) || ColIsSummary(iCol));

        protected virtual string FormatMatrixElement(int iValue)
            => (iValue > 0) ? $"{ iValue, -3 }" : "   ";
    }
}

