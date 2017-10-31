using System;
using System.Linq;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Proxies;
using AVF.MemberManagement.StandardLibrary.Tables;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Extensions.Logging;

namespace AVF.MemberManagement.StandardLibrary.Archive
{
    //example for filter via proxy

    public class UsersProxy : Proxy<TblUsers, User>//, IUsersProxy
    {
        private const string Uri = TblUsers.TableName;

        public UsersProxy(ILogger logger, IPhpCrudApiService phpCrudApiService) : base(logger, phpCrudApiService)
        {
        }

        public async Task<User> GetUserAsync(string username)
        {
            //Make Users a cached repo and
            //move this to repo

            if (username == null) throw new ArgumentNullException(nameof(username));

            var uri = $"{Uri}?filter=Username,eq,{username}";

            var foundUsers = (await _phpCrudApiService.GetDataAsync<TblUsers>(uri)).Rows;

            return foundUsers.Any() ? foundUsers.Single() : null;
        }
    }
}

