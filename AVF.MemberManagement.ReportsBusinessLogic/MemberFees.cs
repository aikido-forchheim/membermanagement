using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.ReportsBusinessLogic
{
    public class MemberFees
    {
        public static decimal GetMemberFee(Mitglied member)
        {
            decimal decJahresbeitrag = 0;
            if (member.Faktor > 0)
            {
                int iProzentsatz = Globals.DatabaseWrapper.Familienrabatt(member);
                if (iProzentsatz > 0)
                {
                    decimal decStdJahresbeitrag = Globals.DatabaseWrapper.BK(member).Beitrag;
                    decJahresbeitrag = decStdJahresbeitrag * iProzentsatz / 100;
                }
            }
            return decJahresbeitrag;
        }
    }
}
