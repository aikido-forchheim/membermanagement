using System.Collections.Generic;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface ITable<T>
    {
        string Uri { get; set; }
        List<T> Rows { get; set; }
    }
}