using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Models.Tbo;

namespace AVF.MemberManagement.StandardLibrary.Models.Tables
{
    internal class TblUsers
    {
        public const string TableName = "Users";

        public List<User> Users
        {
            get;
            set;
        }
    }
}
