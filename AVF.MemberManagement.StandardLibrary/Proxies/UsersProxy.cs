using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using Microsoft.Extensions.Logging;

namespace AVF.MemberManagement.StandardLibrary.Proxies
{
    public class UsersProxy : IUsersProxy
    {
        private readonly ILogger _logger;
        private readonly IPhpCrudApiService _phpCrudApiService;

        private readonly string _uri = $"Users";

        private List<User> _users;

        public UsersProxy(ILogger logger, IPhpCrudApiService phpCrudApiService)
        {
            _logger = logger;
            _phpCrudApiService = phpCrudApiService;
        }

        public async Task UpdateUserAsync(User user)
        {
            await _phpCrudApiService.UpdateDataAsync(_uri + "/" + user.UserId, user);

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

                var maxUserId = (from u in _users select u.UserId).Max();
                nextId = maxUserId + 1;
            }

            var newUser = new User
            {
                UserId = nextId,
                Active = true,
                Username = newUserName
            };

            await _phpCrudApiService.SendDataAsync(_uri, newUser);

            await GetUsersAsync();
        }

        public async Task<User> GetUserAsync(string username)
        {
            if (username == null) throw new ArgumentNullException(nameof(username));

            var uri = $"{_uri}?filter=Username,eq,{username}";

            _users = (await _phpCrudApiService.GetDataAsync<UsersWrapper>(uri)).Users;

            return _users.Any() ? _users.Single() : null;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            _users = (await _phpCrudApiService.GetDataAsync<UsersWrapper>(_uri)).Users;

            _logger.LogInformation(_users.Count + " Users loaded");

            return _users;
        }
    }
}

