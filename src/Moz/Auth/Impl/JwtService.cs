using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moz.Core.Options;

namespace Moz.Auth.Impl
{
    public class JwtService:IJwtService
    {
        private readonly IOptions<MozOptions> _mozOptions;

        public JwtService(IOptions<MozOptions> mozOptions)
        {
            _mozOptions = mozOptions;
        }


        public string GenerateJwtToken(string memberUId)
        {
            var claims = new[] 
            {
                new Claim(JwtRegisteredClaimNames.Jti,memberUId),
                new Claim(JwtRegisteredClaimNames.Exp,DateTime.Now.AddDays(90).ToUniversalTime().ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_mozOptions.Value.EncryptKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                "https://ut32.com",
                "moz_application",
                claims,
                expires: DateTime.Now.AddDays(90),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}