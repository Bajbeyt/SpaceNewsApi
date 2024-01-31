using Entities.Models;
using Entities.ModelDTO;

namespace Services.Contracts;

public interface IRolesService
{
    Task<IEnumerable<Roles>> GetAllRoles(bool trackChanges);
    Task<Roles> GetRoles(int id,bool trackChanges);
    Task<Roles> CreateRole(RolesDto roleDto);
    void UpdateRole(RolesDto roleDto);
    void DeleteRole(int id);
    Task<string> AddRoleToUser(int userId, string roleName);
}