using AutoMapper;
using Entities.ModelDTO;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Repository.Contracts;
using Services.Contracts;

namespace Services;

public class RolesService:IRolesService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _user;
    private readonly RoleManager<Roles> _role;

    public RolesService(IRepositoryManager repository, IMapper mapper, UserManager<User> user, RoleManager<Roles> role)
    {
        _repository = repository;
        _mapper = mapper;
        _user = user;
        _role = role;
    }

    public async Task<IEnumerable<Roles>> GetAllRoles(bool trackChanges)
    {
        var roles= await _repository.Roles.GenericRead(trackChanges);
        return roles;
    }

    public async Task<Roles> GetRoles(int id, bool trackChanges)
    {
        var roles =_repository.Roles.GetRoles(id, false);
        return roles;
    }

    public async Task<Roles> CreateRole(RolesDto roleDto)
    {
        var roles = _mapper.Map<Roles>(roleDto);
        await _repository.Roles.GenericCreate(roles);
        _repository.Save();
        return roles;
    }

    public void UpdateRole(RolesDto roleDto)
    {
        var roles = _repository.Roles.GetRoles(roleDto.Id, false);
        if (roles!=null)
        {
            _mapper.Map<Roles>(roles);
            _repository.Roles.GenericUpdate(roles);
            _repository.Save();
        }
    }

    public void DeleteRole(int id)
    {
        var roles = _repository.Roles.GetRoles(id, false);
        if (roles!=null)
        {
            _mapper.Map<Roles>(roles);
            _repository.Roles.GenericDelete(roles);
            _repository.Save();
        }
    }

    public async Task<string> AddRoleToUser(int userId, string roleName)
    {
        var userIdString = userId.ToString();
        var user = await _user.FindByIdAsync(userIdString);
        if (userId==null)
        {
            return "Kullanıcı bulunamadı.";
        }
        
        if (string.IsNullOrWhiteSpace(roleName)) //IsNullOrWhiteSpace girilen değerin null olup olamamsına ve boşluk kontrolü için kullanılıyor
        {
            return "Geçersiz rol adı.";
        }

        var role = _role.RoleExistsAsync(roleName);//veritabanında girilen roleName ile eşleşeni arayan metod
        if (role==null)
        {
            return "Girdiğiniz Değerde Rol Yoktur";
        }
        var result = _user.AddToRoleAsync(user, roleName);
        return "kullanıcıya rol ataması gerçekleşmiştir";
    }
}