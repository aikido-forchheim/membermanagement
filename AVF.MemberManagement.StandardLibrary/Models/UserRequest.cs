using System;

namespace AVF.MemberManagement.StandardLibrary.Models
{
    public class UserRequest
    {
        public DateTime RequestTime { get; set; }
        public User User { get; set; }
    }
}
