using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Models;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface IKursModelService
    {
        Task<Class> GetAsync(Kurs kurs);
    }
}