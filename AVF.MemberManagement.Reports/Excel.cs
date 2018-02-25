using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class ExcelExport
    {
        public void Export2Excel
        ( 
            DataGridView dgv,
            int rowStart,   // 1 means data begins in row 1
            int colStart,   // 1 means data begins in column A
            string fileName // without file extension 
        )
        {
            Excel.Application excel = new Excel.Application();
            Workbook workBook = excel.Workbooks.Open("Graduierungsliste", ReadOnly:false);
//            Workbook workBook = excel.Workbooks.Add(Type.Missing);
            Worksheet workSheet = workBook.ActiveSheet;

            string fontName = dgv.DefaultCellStyle.Font.Name;
            excel.StandardFont = fontName;

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                Range range = workSheet.Cells[rowStart, colStart + col.Index];
                range.Value = col.HeaderCell.Value;
//                range.Interior.Color = ColorTranslator.ToOle(col.HeaderCell.Style.BackColor);
            }

            foreach ( DataGridViewRow row in dgv.Rows )
            { 
                foreach ( DataGridViewCell cell in row.Cells )
                {
                    Range range = workSheet.Cells[rowStart + row.Index + 1, colStart + cell.ColumnIndex];
                    range.Value = cell.Value;
                    range.Font.Color = ColorTranslator.ToOle(cell.Style.ForeColor);
                }
            }

//            workSheet.Rows.AutoFit();
//            workSheet.Columns.AutoFit();
//            workSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
//            workBook.SaveAs( fileName );
            workBook.Save();
            workBook.Close();
            excel.Quit();
        }
    }
}
