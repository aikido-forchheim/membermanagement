using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Models.Tbo;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface IBeitragsklassenProxy
    {
        Task<Beitragsklasse> GetAsync(int id);
        Task<List<Beitragsklasse>> GetAsync();
        Task<string> UpdateAsync(Beitragsklasse test);
    }
}