using System;
using System.Windows.Forms;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public partial class ReportTrainingsParticipation : ReportBase
    {
        protected VerticalAxis P_yAxis { get; private set; }
        protected HorizontalAxis P_xAxis { get; private set; }
        protected AxisType P_xAxisType { get; private set; }
        protected AxisType P_yAxisType { get; private set; }
        protected TrainingParticipationModel P_tpModel { get; private set; }

        protected AxisTypeMember P_axisTypeMember { get; private set; }

        protected ReportDescriptor m_reportDescriptor;

        public ReportTrainingsParticipation( ReportDescriptor desc )
        {
            m_reportDescriptor = desc;
            InitializeReportTrainingsParticipation(); // creates DataGridView ...
            P_labelReportName.Text = "Trainingsteilnahme";
            P_labelZeitraum.Text = Globals.GetTimeRangeDescription(m_reportDescriptor.P_timeRange);

            P_dataGridView.Sorted += new EventHandler(delegate (object s, EventArgs e) { Sorted(s, e); });
            P_yearSelector.Minimum = Globals.DatabaseWrapper.GetStartValidData().Year;
            P_yearSelector.Maximum = DateTime.Now.Year;
            P_yearSelector.Value = m_reportDescriptor.P_timeRange.P_datStart.Year;
            P_yearSelector.ValueChanged += new System.EventHandler(YearSelectionChanged);

            P_xAxis = new HorizontalAxis();
            P_yAxis = new VerticalAxis();

            P_xAxisType = (AxisType)Activator.CreateInstance(m_reportDescriptor.P_xAxisType, m_reportDescriptor);
            P_yAxisType = (AxisType)Activator.CreateInstance(m_reportDescriptor.P_yAxisType, m_reportDescriptor);

            P_xAxis.P_startIndex = P_yAxisType.HeaderStrings.Count + 1; ;
            P_yAxis.P_startIndex = 0;

            P_tpModel = new TrainingParticipationModel(m_reportDescriptor, P_xAxisType, P_yAxisType);

            ReportFormPopulate();
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

        private void DisplayTrainer(int idTrainer)
            => P_labelTrainer.Text = $"Trainer: {AxisTypeMember.GetFullName(Globals.DatabaseWrapper.MitgliedFromId(idTrainer))}";

        private void DisplayCourse(int idCourse)
        {
            if (idCourse != Globals.ALL_COURSES)
            {
                P_labelKurs.Text = "Kurs: " + String.Join(" ", AxisTypeCourse.GetDesc(idCourse));
                int idTrainer = Globals.DatabaseWrapper.KursFromId(idCourse).Trainer;
                if (idTrainer > 0)
                    DisplayTrainer(idTrainer);
            }
        }

        protected override void ReportFormPopulate()    // Fill cells of DataGridView
        {
            P_xAxis.Initialize( P_xAxisType );
            P_yAxis.Initialize( P_yAxisType );
            ReportFormSize();

            if (m_reportDescriptor.P_idMember != Globals.ALL_MEMBERS)
                P_labelMember.Text = AxisTypeMember.GetFullName(Globals.DatabaseWrapper.MitgliedFromId(m_reportDescriptor.P_idMember));

            P_labelMonat.Text = AxisTypeMonth.GetDesc(m_reportDescriptor, m_reportDescriptor.P_nrPeriod)[0];

            DisplayCourse(m_reportDescriptor.P_idCourse);

            if (m_reportDescriptor.P_idTraining != Globals.ALL_TRAININGS)
            {
                Training training      = Globals.DatabaseWrapper.TrainingFromId(m_reportDescriptor.P_idTraining);
                P_labelReportName.Text = $"Training am {Globals.DatabaseWrapper.WeekDay(training.WochentagID)} den ";
                DisplayTrainer(training.Trainer);
                P_labelZeitraum.Text   = AxisTypeTraining.GetDate(training, '.');
                P_labelMember.Text     = AxisTypeTraining.GetTime(training);
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
                P_tpModel.ForAllRows   // summary column
                (
                    action: iModelRow =>
                    {
                        DataGridViewCell cell = P_dataGridView[P_dataGridView.ColumnCount - 1, iDgvRow];
                        cell.Value = P_tpModel.GetRowSum(iModelRow);
                        cell.ToolTipText = P_yAxisType.MouseAxisEvent(P_yAxis.GetDbIdFromDgvIndex(iDgvRow), false);
                        iDgvRow++;
                    },
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
            int sumSum   = 0;
            int rowNrSum = P_dataGridView.RowCount - 1;
            int iDgvCol  = P_xAxis.P_startIndex;
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
            }
            else if (IsSummaryRow(row))
            {
                return String.Empty;  // TODO: add some action
            }
            else // data row
            {
                if (ColIsInMainArea(col))
                {
                    return String.Empty; // TODO: add some action
                }
                else // key or summary 
                {
                    return action ? P_yAxisType.MouseAxisEvent(P_yAxis.GetDbIdFromDgvIndex(row), action) : String.Empty;
                }
            }
            return String.Empty;
        }

        private void YearSelectionChanged(object sender, EventArgs e)
        {
            int iYear = (int)((NumericUpDown)sender).Value;
            if ((P_yearSelector.Minimum <= iYear) && (iYear <= P_yearSelector.Maximum))
            {
                ReportMain.P_formMain.NewTrainingsParticipationPanel(m_reportDescriptor, timeRange: new TimeRange(iYear));
            }
        }

        private bool ColIsKeyArea(int iCol)
            => iCol < P_yAxisType.HeaderStrings.Count + 1;

        private bool ColIsSummary(int iCol)
            => iCol == P_dataGridView.ColumnCount - 1;

        private bool ColIsInMainArea(int iCol)
            => !(ColIsKeyArea(iCol) || ColIsSummary(iCol));

        protected virtual string FormatMatrixElement(int iValue)
            => (iValue == 0) 
               ?   " " 
               :   ( P_xAxisType.GetType() == typeof(AxisTypeTraining) )
                   ?   "X"
                   :   $"{ iValue,-3 }";
    }
}
