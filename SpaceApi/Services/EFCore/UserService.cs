using AutoMapper;
using Entities.ModelDTO;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Repository.Contracts;
using Services.Contracts;

namespace Services;

public class UserService:IUserService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Roles> _roleManager;
    private readonly User? user;
    public UserService(IRepositoryManager repositoryManager,IMapper mapper
        , UserManager<User> userManager, RoleManager<Roles> roleManager
        )
    {
        _repository = repositoryManager;
        _mapper = mapper;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IEnumerable<User>> GetAllUsers(bool trackChanges)
    {
        var users= await _repository.User.GenericRead(trackChanges);
        return users.ToList();
    }

    public async Task<User> GetUsers(int id, bool trackChanges)
    {
        var users = _repository.User.GetUser(id, trackChanges);
        return users;
    }

    public async Task<User> CreateUser(UserForRegistrationDto userForRegistrationDto)
    {
        var user = _mapper.Map<User>(userForRegistrationDto);
        await _repository.User.GenericCreate(user);
        // _repositoryManager.Save();
        return user;
    }

    public void UpdateUser(int id,UserForRegistrationDto userForRegistrationDto)
    {
        var user = _repository.User.GetUser(id, false);
        if (user!=null)
        {
            _mapper.Map<UserForRegistrationDto>(userForRegistrationDto);
            _repository.Save();
        }
    }

    public void DeleteUser(int id)
    {
        var user = _repository.User.GetUser(id, false);
        // _repositoryManager.User.GenericDelete(user);
        if (user!=null)
        {
            _repository.User.GenericDelete(user);
            _repository.Save();
        }
    }

    // public async Task AddRoleToUser(string userName, string roleName)
    // {
    //     var usersName = _userManager.FindByNameAsync(userName);
    //     if (usersName==null)
    //     {
    //         return; //eğer nullsa işlemi durduracak
    //     }
    //     var role = _roleManager.FindByNameAsync(roleName);
    //     if (role==null)
    //     {
    //         return;
    //     }
    //
    //     var addToUserRole= await _userManager.AddToRoleAsync(user, roleName);
    //     if (addToUserRole.Succeeded==null)
    //     {
    //         return;
    //     }
    //     //Succeeded sonucun başarılı olup olmadığını taşır
    // }
}