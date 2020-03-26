using System;
using Microsoft.IdentityModel.Tokens;

namespace Moz.Auth
{
    public interface IJwtService
    {
        TokenInfo GenerateTokenInfo(string memberUId);

        TokenValidationParameters GetTokenValidationParameters();
    }
}