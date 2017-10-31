using AVF.MemberManagement.StandardLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Models.Tables;
using AVF.MemberManagement.StandardLibrary.Models.Tbo;

namespace AVF.MemberManagement.StandardLibrary.Proxies
{
    public class BeitragsklassenProxy : ProxyBaseInt<TblBeitragsklassen, Beitragsklasse>, IBeitragsklassenProxy
    {
        public BeitragsklassenProxy(IPhpCrudApiService phpCrudApiService) : base(phpCrudApiService, TblBeitragsklassen.TableName)
        {
        }

        public new Task<string> UpdateAsync(Beitragsklasse test)
        {
            throw new NotSupportedException();
        }
    }
}
