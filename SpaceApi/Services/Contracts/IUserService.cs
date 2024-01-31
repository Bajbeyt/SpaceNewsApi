using Entities.ModelDTO;
using Entities.Models;

namespace Services.Contracts;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsers(bool trackChanges);
    Task<User> GetUsers(int id,bool trackChanges);
    Task<User> CreateUser(UserForRegistrationDto userForRegistrationDto);
    void UpdateUser(int id,UserForRegistrationDto userForRegistrationDto);
    void DeleteUser(int id);
    // Task AddRoleToUser(string userName, string roleName);
}