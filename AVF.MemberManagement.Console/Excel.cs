using System;
using Microsoft.Office.Interop.Excel;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Console
{
    class Excel
    {
        private void WriteMatrix(Worksheet ws, string[,] matrix)
        {
            for (int iRow = 0; iRow < matrix.GetLength(0); ++iRow)
            {
                for (int iCol = 0; iCol < matrix.GetLength(1); ++iCol)
                {
                    Range range = ws.Cells[iRow + 1, iCol + 1];
                    range.Value = matrix[iRow, iCol];
 //                   range.Font.Color = 0x000000FF;
                    range.Interior.Color = 0x000000FF;
                }
            }
            ws.Rows[1].RowHeight = 25;
            ws.Rows[2].RowHeight = 70;
        }

        public void Test(DatabaseWrapper db)
        {
            System.Data.DataTable table = new System.Data.DataTable();

            Application excel = new Application();
            Workbook    workBook = excel.Workbooks.Add(Type.Missing);
            workBook.ActiveSheet.Name = "Testtt";
            Worksheet workSheet = workBook.ActiveSheet;

            int iJahr = 2017;
            DateTime datStart = new DateTime(iJahr, 1, 1);
            DateTime datEnd = new DateTime(iJahr, 12, 31);
            ReportMemberVsCourse tp = new ReportMemberVsCourse(db, datStart, datEnd);

            string[,] matrix = tp.GetMatrix();
            WriteMatrix(workSheet, matrix);

            workBook.SaveAs(Filename: "Testttt");
            excel.Quit();
        }
        
    }
}
