using Entities.Models;
using Repository.Contracts;

namespace Repository.EFCore;

public class RolesRepository:RepositoryBase<Roles>, IRolesRepository
{
    public RolesRepository(RepositoryContext context) : base(context)
    {
    }

    public Roles GetRoles(int id, bool trackChanges)
    {
        return GenericReadExpression(trackChanges, x => x.Id.Equals(id)).SingleOrDefault();
    }
}