using Entities.Models;

namespace Repository.Contracts;

public interface IRolesRepository:IRepositoryBase<Roles>
{
    Roles GetRoles(int id, bool trackChanges);
}