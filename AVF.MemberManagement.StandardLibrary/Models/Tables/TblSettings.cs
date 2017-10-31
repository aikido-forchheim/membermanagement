using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Models.Tbo;

namespace AVF.MemberManagement.StandardLibrary.Models.Tables
{
    public class TblSettings
    {
        public const string TableName = "Settings";

        public List<Setting> Settings { get; set; }
    }
}
