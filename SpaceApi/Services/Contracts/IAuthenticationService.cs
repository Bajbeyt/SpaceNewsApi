using Entities.ModelDTO;
using Microsoft.AspNetCore.Identity;

namespace Services.Contracts;

public interface IAuthenticationService
{
    //kullanıcı kayıt imzası
    Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistrationDto);
    //kullanıcı doğrulama işlemi
    Task<bool> ValidateUser(UserForAuthenticationDto userForAuthenticationDto);
    //tokken oluşturma metodu
    Task<TokenDto> CreateTokken(bool populateExp);

    Task<TokenDto> RefreshToken(TokenDto tokenDto);
}