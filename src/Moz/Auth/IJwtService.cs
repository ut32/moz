using System;
using Microsoft.IdentityModel.Tokens;

namespace Moz.Auth
{
    public interface IJwtService
    {
        DateTime ExpireDateTime { get; set; } 
        
        string GenerateRefreshToken(string memberUId); 
        
        string GenerateJwtToken(string memberUId);

        TokenValidationParameters GetTokenValidationParameters();
    }
}