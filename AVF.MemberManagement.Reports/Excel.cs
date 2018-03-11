using System.IO;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class ExcelExport
    {
        public static void Export2Excel
        ( 
            DataGridView dgv,
            int    rowStart,     // 1 means data begins in row 1
            int    colStart,     // 1 means data begins in column A
            string reportName    // internal name of report
        )
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = "Desktop";
            dialog.Title = "Bericht als Excel-Datei speichern";
            dialog.CheckPathExists = true;
            dialog.DefaultExt = "xlsx";
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Excel.Application excel = new Excel.Application();

                string propertyName = "Last" + reportName;
                string strPathLast  = Reports.Properties.Settings.Default[propertyName].ToString();

                Workbook  workBook  = (File.Exists(strPathLast)) ? excel.Workbooks.Open(strPathLast) : excel.Workbooks.Add();                
                Worksheet workSheet = workBook.ActiveSheet;

                // delete any old content 

                Range last  = workSheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell);
                Range range = workSheet.get_Range(workSheet.Cells[rowStart][colStart], last);
                range.Delete();

                // copy new data to workSheet

                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    workSheet.Cells[rowStart, colStart + col.Index].Value = col.HeaderCell.Value;
                }

                foreach ( DataGridViewRow row in dgv.Rows )
                { 
                    foreach ( DataGridViewCell cell in row.Cells )
                    {
                        workSheet.Cells[rowStart + row.Index + 1, colStart + cell.ColumnIndex].Value = cell.Value;
                    }
                }

                workSheet.Rows.AutoFit();
                workSheet.Columns.AutoFit();

                workBook.SaveAs(dialog.FileName);

                Reports.Properties.Settings.Default[propertyName] = dialog.FileName;
                Reports.Properties.Settings.Default.Save();

                workBook.Close();
                excel.Quit();
            }
        }
    }
}
