using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartValley.WebApi.Admin
{
    public class MockAdminService : IAdminService
    {
        private List<string> admins = new List<string> { "0xb5662f4eD85C0a3eFc0704A592D487e9C50E3C67", "Петя", "Саша" };

        public Task AddAsync(string address)
        {
            return Task.Run(() => admins.Add(address));
        }

        public Task DeleteAsync(string address)
        {
            return Task.Run(() => admins.Remove(address));
        }

        public Task<IReadOnlyCollection<string>> GetAllAsync()
        {
            return Task.Run(()=> GetAdmins());
        }

        public Task<bool> IsAdminAsync(string address)
        {
            return Task.Run(() => AdminExist(address));
        }
        
        private IReadOnlyCollection<string> GetAdmins()
        {
            return admins;
        }

        private bool AdminExist(string address)
        {
            return admins.Exists(i => i == address);
        }
    }
}
