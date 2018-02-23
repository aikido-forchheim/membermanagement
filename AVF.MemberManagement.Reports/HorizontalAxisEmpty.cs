using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Reports
{
    // HorizontalAxisEmpty is used for one dimensional forms, which have no horizontal axis

    class HorizontalAxisEmpty : HorizontalAxis
    {
        public HorizontalAxisEmpty()
            => P_Hide = true;
    }
}
