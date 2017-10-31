using System;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.StandardLibrary.Models
{
    public class UserRequest
    {
        public DateTime RequestTime { get; set; }
        public User User { get; set; }
    }
}
