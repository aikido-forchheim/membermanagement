using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Models;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface IReadOnlyProxy<T, in TId>
    {
        Task<List<T>> GetAsync();
        Task<T> GetAsync(TId id);

        Task<TablePropertiesBase> GetTablePropertiesAsync();
    }
}
