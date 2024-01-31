using System.Runtime.InteropServices.ComTypes;
using Entities.ModelDTO;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Services.Contracts;

namespace Presentations.Controllers;

[Route("api/[controller]")]
[ApiController]
[ResponseCache(CacheProfileName = "Space")]
public class UserController:ControllerBase
{
    private readonly IServiceManager _service;

    public UserController(IServiceManager service)
    {
        _service = service;
    }
    [HttpGet("get-User-List")]
    public async Task<IActionResult> GetUserList(int id)
    {
        Log.Information($"GetUserList İşlemi {id} üzerine yapıldı");
        
        var userList = await _service.UserService.GetUsers(id, false);
        return Ok(userList);
    }

    [HttpGet("get-All")]
    public async Task<IActionResult> GetUserAllList()
    {
        Log.Information("GetUserAllList İşlemi yapıldı");
        var user= await _service.UserService.GetAllUsers(false);
        if (user==null)
        {
            return NotFound();
        }
        return Ok(user);
    }
    
    [HttpPut("update-User")]
    public IActionResult UpdateUser(int id,UserForRegistrationDto userForRegistrationDto)
    {
        Log.Information($"Update İşlemi {id} yapıldı");
        _service.UserService.UpdateUser(id,userForRegistrationDto);
        return Ok();
    }
    
    [HttpPost("add-User")]
    public async Task<IActionResult> AddUser(UserForRegistrationDto userForRegistrationDto)
    {
        
        if (userForRegistrationDto.Roles == null)
        {
            return NotFound();
        }
        if (userForRegistrationDto == null)
        {
            Log.Information($"Add İşlemi başarısız oludu {userForRegistrationDto}");
            return BadRequest("Geçersiz Kayıt İşlemi");
        }
        await _service.UserService.CreateUser(userForRegistrationDto);
        Log.Information($"Add İşlemi baraşıyla tamamlandı {userForRegistrationDto}");
        return Ok();
    }
    
    [HttpDelete("delete-User/{id}")]
    public IActionResult DeleteUser(int id)
    {
        Log.Information("Delete İşlemi yapıldı");
        _service.UserService.DeleteUser(id);
        return Ok();
    }
    // [HttpPost("add-user-role")]
    // public async Task<IActionResult> AddUserRole(int id, RolesDto rolesDto)
    // {
    //     var roleName = rolesDto.RoleName;
    //
    //     // UserService aracılığıyla kullanıcıyı kaydet
    //     var userResult = await _service.UserService.CreateUser();
    //
    //     if (userResult.Succeeded)
    //     {
    //         // RoleService aracılığıyla kullanıcıya rol ekle
    //         var roleResult = await _service.RolesService.AddRoleToUser(, roleName);
    //
    //         if (roleResult)
    //         {
    //             return Ok($"User '{username}' registered successfully with role '{roleName}'.");
    //         }
    //         else
    //         {
    //             // Rol eklenemediyse, kullanıcıyı geri al
    //             await _service.UserService.DeleteUser(id);
    //             return BadRequest($"Failed to add role '{roleName}' to user '{username}'.");
    //         }
    //     }
    //     else
    //     {
    //         return BadRequest($"Failed to register user '{username}'.");
    //     }
    // }
    // [HttpPost("add-user-role")]
    // public IActionResult AddUserRole([FromBody]UserForRegistrationDto userDto,[FromForm]RolesDto rolesDto)
    // {
    //     var userName = userDto.UserName;
    //     var roleName = rolesDto.RoleName;
    //     var result = _service.UserService.AddRoleToUser(userName, roleName);
    //     if (result==null)
    //     {
    //         return NotFound();
    //     }
    //
    //     return Ok($"'{roleName}' rolü, '{userName}' kullanıcısına başarıyla eklendi.");
    // }
}