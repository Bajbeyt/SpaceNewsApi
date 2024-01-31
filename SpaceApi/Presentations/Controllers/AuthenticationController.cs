using Entities.ModelDTO;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Services.Contracts;

namespace Presentations.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController:ControllerBase
{
    private readonly IServiceManager _serviceManager;
    // private readonly IAuthenticationService _authenticationService;


    public AuthenticationController(IServiceManager serviceManager
        // , IAuthenticationService authenticationService
        )
    {
        _serviceManager = serviceManager;
        // _authenticationService = authenticationService;
    }

    [HttpPost("register-user")]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto user)
    {
        try
        {
            var result = await _serviceManager.AuthenticationService.RegisterUser(user);
            if (result.Succeeded)
            {
                Log.Information("Register İşlemi Başarılı");
                return Ok();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                Log.Information("Register işlemi başarısız oldu");
                return BadRequest();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    [HttpPost("login")]
    public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
    {
        if (!await _serviceManager.AuthenticationService.ValidateUser(user))
        {
            Log.Information($"Login işlemi Başarılı{user}");
            return Unauthorized(); //401 dönüyor
        }
        var tokenDto = await _serviceManager.AuthenticationService.CreateTokken(populateExp: true);
        return Ok(tokenDto);
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
    {
        Log.Information($"Refresh Token işlemi Başarılı");
        var tokenDtoToReturn = await _serviceManager.AuthenticationService.RefreshToken(tokenDto);
        return Ok(tokenDtoToReturn);
    }
}