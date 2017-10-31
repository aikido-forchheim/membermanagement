using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models.Tables;
using AVF.MemberManagement.StandardLibrary.Models.Tbo;

namespace AVF.MemberManagement.StandardLibrary.Proxies
{
    public class TestsProxy : ProxyBaseInt<TblTests, Test>, ITestProxy
    {
        public TestsProxy(IPhpCrudApiService phpCrudApiService) : base(phpCrudApiService, TblTests.TableName)
        {
        }
    }
}
