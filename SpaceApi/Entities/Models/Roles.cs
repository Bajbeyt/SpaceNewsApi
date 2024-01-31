using Microsoft.AspNetCore.Identity;

namespace Entities.Models;

public class Roles:IdentityRole<int>
{
    public string? RoleName{ get; set; }
    
}