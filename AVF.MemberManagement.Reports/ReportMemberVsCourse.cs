using System;
using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    public class ReportMemberVsCourse : ReportBase
    {
        public ReportMemberVsCourse(DatabaseWrapper db, DateTime datStart, DateTime datEnd)
            : base(db, datStart, datEnd)
        {
            m_iNrOfColsOnLeftSide  = 3;   // 3 columns for Mitglieder
            m_iNrOfColsOnRightSide = 1;   // 1 column for row sum

            m_coreReport.Initialize
            (
                datStart,
                datEnd,
                m_db.MaxMitgliedsNr() + 1,   // One additional row for pseudo member with Id = -1
                m_db.MaxKursNr() + 1,         // One additional col for "Lehrgänge"
                tn => true,
                tn => tn.MitgliedID - 1,  // db ids start with 1, array indices with 0
                tn => m_db.KursIdFromTrainingId(tn.TrainingID)
            );

            m_coreReport.SortRows();
        }

        protected override void SizeDataGridView(DataGridView dgv)
        {
            dgv.RowCount = m_coreReport.GetNrOfActiveRows() + 1;  // one footer row
            dgv.ColumnCount = m_coreReport.GetNrOfActiveCols() + m_iNrOfColsOnLeftSide + m_iNrOfColsOnRightSide;
        }

        protected override void FillHeaderRows(DataGridView dgv)
        {
            dgv.Columns[0].HeaderText = "Vorname";
            dgv.Columns[1].HeaderText = "Nachname";
            dgv.Columns[2].HeaderText = "Nr";

            FillCourseHeaderRows(dgv);
        }

        protected override void FillRowHeaderColumns(DataGridView dgv)
        {
            FillMemberRowHeaderColumns(dgv);
        }

        protected override string FormatMatrixElement(int iValue)
        {
            return (iValue > 0) ? $"{ iValue,-3 }" : "   ";
        }
    }
}
