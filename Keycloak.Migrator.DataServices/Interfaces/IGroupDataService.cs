using Keycloak.Net.Models.Groups;
using System;
using System.Linq;

namespace Keycloak.Migrator.DataServices.Interfaces
{
    public interface IGroupDataService
    {
        Task<Group> GetGroup(string realm, string groupId);
        public Task<IEnumerable<Group>> GetGroups(string realm);
        public Task<bool> UpdateGroup(string realm, Group group);
        public Task<bool> DeleteGroup(string realm, Group group);
        public Task<bool> AddGroup(string realm, Group group);
    }
}
