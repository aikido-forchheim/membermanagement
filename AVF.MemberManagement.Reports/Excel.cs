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
            Excel.Application excel = new Excel.Application();

            Workbook workBook = excel.Workbooks.Add(); //             .Open(reportName, ReadOnly:false);
            Worksheet workSheet = workBook.ActiveSheet;

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                Range range = workSheet.Cells[rowStart, colStart + col.Index];
                range.Value = col.HeaderCell.Value;
            }

            foreach ( DataGridViewRow row in dgv.Rows )
            { 
                foreach ( DataGridViewCell cell in row.Cells )
                {
                    Range range = workSheet.Cells[rowStart + row.Index + 1, colStart + cell.ColumnIndex];
                    range.Value = cell.Value;
//                    range.Font.Color = ColorTranslator.ToOle(cell.Style.ForeColor);
                }
            }

            //            workSheet.Rows.AutoFit();
            //            workSheet.Columns.AutoFit();
            //            workBook.SaveAs( fileName );

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = "Desktp";
            dialog.Title = "Bericht als Excel-Datei speichern";
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.DefaultExt = "xlsx";
            dialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            dialog.FilterIndex = 2;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Stream oStream;
                if ((oStream = dialog.OpenFile()) != null)
                {
                    workBook.SaveAs(oStream);
                    oStream.Close();
                }
            }

            workBook.Close();
            excel.Quit();
        }
    }
}
