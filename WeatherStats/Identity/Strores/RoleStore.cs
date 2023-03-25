using Microsoft.AspNetCore.Identity;
using System.Data;

namespace WeatherStats.Identity.Strores
{
    public class RoleStore : IRoleStore<IdentityRole>
    {
        private readonly List<IdentityRole> s_roles = new List<IdentityRole>();

        public Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            var existingRole = s_roles.FirstOrDefault(r => r.Id == role.Id);

            if (existingRole != null)
            {
                return Task.FromResult(IdentityResult.Failed());
            }

            s_roles.Add(role);

            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            var existingRole = s_roles.FirstOrDefault(r => r.Id == role.Id);

            if (existingRole != null)
            {
                s_roles.Remove(existingRole);
            }

            return Task.FromResult(IdentityResult.Success);
        }

        public void Dispose()
        {
        }

        public Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            var existingRole = s_roles.FirstOrDefault(r => r.Id == roleId);

            if (existingRole == null )
            {
                throw new Exception("Role not found");
            }

            return Task.FromResult(existingRole);
        }

        public Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var existingRole = s_roles.FirstOrDefault(r => r.NormalizedName == normalizedRoleName);

            if (existingRole == null)
            {
                throw new Exception("Role not found");
            }

            return Task.FromResult(existingRole);
        }

        public Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new InvalidOperationException();
        }

        public Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken)
        {
            throw new InvalidOperationException();
        }

        public Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            throw new InvalidOperationException();
        }
    }
}
