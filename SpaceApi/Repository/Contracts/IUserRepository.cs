using Entities.Models;

namespace Repository.Contracts;

public interface IUserRepository:IRepositoryBase<User>
{
    User GetUser(int id, bool trackChanges);
}