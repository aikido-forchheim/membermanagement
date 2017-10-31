using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models.Tables;
using AVF.MemberManagement.StandardLibrary.Models.Tbo;
using Microsoft.Extensions.Logging;

namespace AVF.MemberManagement.StandardLibrary.Proxies
{
    public class UsersProxy : IUsersProxy
    {
        private readonly ILogger _logger;
        private readonly IPhpCrudApiService _phpCrudApiService;

        private const string Uri = TblUsers.TableName;

        private List<User> _users;

        public UsersProxy(ILogger logger, IPhpCrudApiService phpCrudApiService)
        {
            _logger = logger;
            _phpCrudApiService = phpCrudApiService;
        }

        public async Task UpdateUserAsync(User user)
        {
            await _phpCrudApiService.UpdateDataAsync(Uri + "/" + user.Id, user);

            await GetUsersAsync();
        }

        public async Task AddNewUserAsync(string newUserName)
        {
            await GetUsersAsync();

            var nextId = 1;
            if (!(_users == null || _users.Count == 0))
            {
                var userNames = (from u in _users select u.Username.ToLower()).ToList();
                if (userNames.Contains(newUserName.ToLower())) return;

                var maxUserId = (from u in _users select u.Id).Max();
                nextId = maxUserId + 1;
            }

            var newUser = new User
            {
                Id = nextId,
                Active = true,
                Username = newUserName
            };

            await _phpCrudApiService.SendDataAsync(Uri, newUser);

            await GetUsersAsync();
        }

        public async Task<User> GetUserAsync(string username)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));

            var uri = $"{Uri}?filter=Username,eq,{username}";

            _users = (await _phpCrudApiService.GetDataAsync<TblUsers>(uri)).Users;

            return _users.Any() ? _users.Single() : null;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            _users = (await _phpCrudApiService.GetDataAsync<TblUsers>(Uri)).Users;

            _logger.LogInformation(_users.Count + " Users loaded");

            return _users;
        }
    }
}

