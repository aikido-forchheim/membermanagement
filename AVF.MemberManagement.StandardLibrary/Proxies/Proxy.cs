using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using Microsoft.Extensions.Logging;

namespace AVF.MemberManagement.StandardLibrary.Proxies
{
    public class Proxy<TTbl, T> : ProxyBase<TTbl, T, int>, IProxy<T> 
        where TTbl : ITable<T>, new()
        where T : IIntId, new()
    {
        public Proxy(ILogger logger, IPhpCrudApiService phpCrudApiService) : base(logger, phpCrudApiService)
        {
        }

        public async new Task<TableProperties> GetTablePropertiesAsync()
        {
            var tablePropertiesBase = await base.GetTablePropertiesAsync();

            var tableProperties = new TableProperties
            {
                LastPrimaryKey = int.Parse(tablePropertiesBase.LastPrimaryKey),
                RowCount = tablePropertiesBase.RowCount
            };

            return tableProperties;
        }
    }
}
