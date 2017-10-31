using System.Collections.Generic;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface ITable<T>
    {
        List<T> Rows { get; set; }
    }
}