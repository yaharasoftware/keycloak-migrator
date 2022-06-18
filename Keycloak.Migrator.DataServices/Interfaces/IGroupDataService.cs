using Keycloak.Net.Models.Groups;
using System;
using System.Linq;

namespace Keycloak.Migrator.DataServices.Interfaces
{
    public interface IGroupDataService
    {
        public Task<IEnumerable<Group>> GetGroups(string realm);
        public Task<bool> UpdateRole(string realm, Group group);
        public Task<bool> DeleteRole(string realm, Group group);
        public Task<bool> AddRole(string realm, Group group);
    }
}
