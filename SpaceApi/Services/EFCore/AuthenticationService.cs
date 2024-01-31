using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Entities.Exeption;
using Entities.ModelDTO;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Contracts;

namespace Services;

public class AuthenticationService:IAuthenticationService
{
    // private readonly ILoggerService _logger;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private User? _user;
        public AuthenticationService(IMapper mapper,
            UserManager<User> userManager,
            IConfiguration configuration)
        {
            // _logger = loger;
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng=RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public async Task<TokenDto> CreateTokken(bool populateExp)
        {
            //Appsettings teki konfigrasyon bilgileri alınacak
            var signinCredentials = GetSigninCreatials();
            //Claims verileri alınacak(Rol ve yetkiler)
            var claims = GetClaims();
            //Alınan konfigrasyon ve claims bilgilerine göre token oluşturulacak
            var tokenOptions = GeneratedTokenOptions(signinCredentials, claims);

            var refreshToken = GenerateRefreshToken();
            _user.RefreshTokenExpiryTime=DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(_user);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(await tokenOptions);
            return new TokenDto()
            {
                AccesssToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccesssToken);
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            if (user is null ||
                user.RefreshToken != tokenDto.RefreshToken ||
                    user.RefreshTokenExpiryTime<=DateTime.Now)
                throw new RefreshTokenBadRequestException();
                _user = user;
                return await CreateTokken(populateExp: false);
            
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["secretKey"];

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["validIssuer"],
                ValidAudience = jwtSettings["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters,
                out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
               // as gibi davranmasını sağlar (Başarısız olma durumunda null gelecek)
            if(jwtSecurityToken is null ||
               //Algoritma kontrolü(karşılaştırma)
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token.");
            }
            return principal;
        }
        private async Task<SecurityToken> GeneratedTokenOptions(SigningCredentials signinCredentials, Task<List<Claim>> claimsTask)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
        
            // claimsTask'i await kullanarak beklet
            var claims = await claimsTask;
        
            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings["ValidateIssuer"],
                audience: jwtSettings["ValidateAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["Expires"])),
                signingCredentials: signinCredentials
            );
        
            return tokenOptions;
        }

    
        // private SecurityToken GeneratedTokenOptions(SigningCredentials signinCredentials, Task<List<Claim>> claims)
        // {
        //     var jwtSettings = _configuration.GetSection("JwtSettings");
        //     var tokenOptions = new JwtSecurityToken(
        //         issuer: jwtSettings["ValidateIssuer"],
        //         audience: jwtSettings["ValidateAudience"],
        //         claims:(IEnumerable<Claim>) claims,
        //         expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["Expires"])), //şuanki zamana ekliyorum
        //         signingCredentials: signinCredentials
        //         );
        //     return tokenOptions;
        // }

        private async Task<List<Claim>> GetClaims()    //Claims verileri alınacak(role ve yetkiler)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,_user.UserName)
            };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private SigningCredentials GetSigninCreatials()		//şifrelenmişbir key oluşturuldu
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = System.Text.Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);
            var secret = new SymmetricSecurityKey(key); //zorunlu değil fakat güvenliği arttırmak için yazdık
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);//keyimi burada şifreledim
        }

        public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userForRegistrationDto)
        {
            var user = _mapper.Map<User>(userForRegistrationDto);
            var result = await _userManager.CreateAsync(user, userForRegistrationDto.Password);
            
            return result;
        }

        public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuthenticationDto)
        {
            _user = await _userManager.FindByNameAsync(userForAuthenticationDto.UserName);
            var result = (_user is not null && await _userManager.CheckPasswordAsync(_user, userForAuthenticationDto.Password));
            if (!result)
            {
                // _logger.LogError("Kullanıcı Adı veya Parolanız hatalı");
                Console.WriteLine("Kullanıcı Adı veya Parolanız hatalı");
            }
            return result;
        }
}