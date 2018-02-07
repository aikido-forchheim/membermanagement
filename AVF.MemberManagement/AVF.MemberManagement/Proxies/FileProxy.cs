using System;
using System.Linq;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using AVF.MemberManagement.StandardLibrary.Options;
using AVF.MemberManagement.StandardLibrary.Proxies;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AVF.MemberManagement.Proxies
{
    public class FileProxy<TTbl, T> : FileProxyBase<TTbl, T, int>, IProxy<T>
        where TTbl : ITable<T>, new()
        where T : IIntId
    {
        public FileProxy(IOptions<FileProxyOptions> fileProxyOptions) : base(fileProxyOptions)
        {
        }

        public override async Task<int> CreateAsync(T obj)
        {
            var list = await GetDataFromJsonFileAsync();

            if (list.Select(l => l.Id).Contains(obj.Id) || obj.Id == 0)
            {
                throw new IndexOutOfRangeException("T obj needs to have an Id not equals zero or existing in the list already");
            }

            list.Add(obj);

            await SaveDataToJsonFileAsync(list);

            return 0;
        }

        public async new Task<TableProperties> GetTablePropertiesAsync()
        {
            var list = await GetDataFromJsonFileAsync();

            var tableProperties = new TableProperties
            {
                LastPrimaryKey = list.Select(obj => obj.Id).Max(),
                RowCount = list.Count
            };

            return tableProperties;
        }
    }
}
