using System.ComponentModel.DataAnnotations;

namespace Entities.ModelDTO;

public class UserForAuthenticationDto
{
    [Required(ErrorMessage = "User is requred.")]
    public string? UserName { get; init; }

    [Required(ErrorMessage = "Password is required.")]
    public string? Password { get; init; }
    // init = ilk değeri aldıktan sonra değiştirilemez
}