using System.Windows.Forms;
using AVF.MemberManagement.ReportsBusinessLogic;

namespace AVF.MemberManagement.Reports
{
    class HorizontalAxisCourses : HorizontalAxis
    {
        public HorizontalAxisCourses()
            => P_AxisType = new AxisTypeCourse();

        public override int GetNrOfDgvCols(TrainingParticipationModel tpModel)
            => DatabaseIdRange();  

        protected override int GetModelIndexFromId(int idKurs)
            => Globals.DatabaseWrapper.m_kurs.FindIndex(kurs => idKurs == kurs.Id);

        protected override int GetIdFromModelIndex(int iModelCol)
            => Globals.DatabaseWrapper.m_kurs[iModelCol].Id;
    }
}
