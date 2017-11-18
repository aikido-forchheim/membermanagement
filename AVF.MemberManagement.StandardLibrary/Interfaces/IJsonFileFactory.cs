using System.Collections.Generic;
using System.Threading.Tasks;
using PCLStorage;

namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface IJsonFileFactory
    {
        Task<List<IFile>> RefreshFileCache();
    }
}