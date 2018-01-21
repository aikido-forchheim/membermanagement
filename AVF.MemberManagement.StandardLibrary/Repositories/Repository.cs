using System.Collections.Generic;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;

namespace AVF.MemberManagement.StandardLibrary.Repositories
{
    public class Repository<T> : IRepository<T> where T : IIntId
    {
        private readonly IProxy<T> _proxy;

        public Repository(IProxy<T> proxy)
        {
            _proxy = proxy;
        }

        public virtual async Task<int> CreateAsync(T obj)
        {
            if (obj.Id == 0)
            {
                var tableProperties = await _proxy.GetTablePropertiesAsync();
                var newId = tableProperties.LastPrimaryKey + 1;
                obj.Id = newId;
            }

            return await _proxy.CreateAsync(obj);
        }

        public virtual async Task<List<T>> GetAsync()
        {
            return await _proxy.GetAsync();
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await _proxy.GetAsync(id);
        }

        //Update

        public virtual async Task<int> DeleteAsync(T obj)
        {
            return await _proxy.DeleteAsync(obj);
        }
    }
}
