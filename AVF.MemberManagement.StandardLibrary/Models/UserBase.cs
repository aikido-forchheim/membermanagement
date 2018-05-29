namespace AVF.MemberManagement.StandardLibrary.Models
{
    public class UserBase
    {
        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public bool Active
        {
            get;
            set;
        }

        public int Mitgliedsnummer
        {
            get;
            set;
        }
    }
}
