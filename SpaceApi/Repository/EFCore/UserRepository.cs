using Entities.Models;
using Repository.Contracts;

namespace Repository.EFCore;

public class UserRepository:RepositoryBase<User>,IUserRepository
{
    public UserRepository(RepositoryContext context) : base(context)
    {
    }

    public User GetUser(int id, bool trackChanges)
    {
        return GenericReadExpression(trackChanges, x => x.Id.Equals(id)).SingleOrDefault();
    }
}