using Microsoft.AspNetCore.Http;

namespace Entities.Exeption;

public class RefreshTokenBadRequestException:BadHttpRequestException
{
    public RefreshTokenBadRequestException():base($"Invalid client request.The tokenDto has some invalid values.")
    {
    }
}