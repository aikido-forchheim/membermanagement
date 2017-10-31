using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;

namespace AVF.MemberManagement.StandardLibrary.Repositories
{
    public class CachedRepository<T> : Repository<T> where T : IIntId
    {
        private List<T> _cache = new List<T>();

        public CachedRepository(IProxy<T> proxyBaseInt) : base(proxyBaseInt)
        {
        }

        public override async Task<List<T>> GetAsync()
        {
            _cache = await base.GetAsync();
            return _cache;
        }

        public override async Task<T> GetAsync(int id)
        {
            if (_cache.Count == 0)
            {
                _cache = await base.GetAsync();
            }

            return _cache.Single(c => c.Id == id);
        }
    }
}
