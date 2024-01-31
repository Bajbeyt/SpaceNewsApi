using Entities.ModelDTO;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Services.Contracts;

namespace Presentations.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RolesController:ControllerBase
{
    private readonly IServiceManager _service;

    public RolesController(IServiceManager service)
    {
        _service = service;
    }
    [HttpGet("get-Role/{id}")]
    public async Task<IActionResult> GetRole(int id)
    {
        var roleList = await _service.RolesService.GetRoles(id, false);
        return Ok(roleList);
    }
    [HttpPut("update-Roles/{id}")]
    public IActionResult UpdateRoles(RolesDto rolesDto)
    {
        if (rolesDto!=null)
        {
            _service.RolesService.UpdateRole(rolesDto);
            return Ok();    
        }
        return NotFound();
    }

    [HttpPost("add-Roles")]
    public async Task<IActionResult> AddRoles(RolesDto rolesDto)
    {
        if (rolesDto==null)
        {
            return BadRequest("Geçersiz Role Kaydı");
        }

        await _service.RolesService.CreateRole(rolesDto);
        return Ok();
    }

    [HttpDelete("delete-Roles/{id}")]
    public IActionResult DeleteRoles(int id)
    {
        _service.RolesService.DeleteRole(id);
        return Ok();
    }

    [HttpGet("get-All-Role")]
    public IActionResult GetAllRole()
    {
        var role = _service.RolesService.GetAllRoles(false);
        return Ok(role);
    }

    [HttpPost("add-role-user")]
    public async Task<IActionResult> AddRoleToUser(int id,RolesDto rolesDto)
    {
        Log.Information("AddRollToUser İşlemi yapıldı");
        var roleName = rolesDto.RoleName;
        if (roleName != null)
        {
            var data = await _service.RolesService.AddRoleToUser(id, roleName);

            if (data!=null)
            {
                return Ok(roleName);
            }
            else
            {
                
                return NotFound("Rol Ataması Gerçekleşmedi, bir hata oluştu");
            }
        }
        else
        {
            return BadRequest("Geçersiz rol adı");
        }
    }
}