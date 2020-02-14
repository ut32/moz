using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moz.Core.Options;
using Moz.Exceptions;
using Moz.Service.Security;

namespace Moz.Auth.Impl
{
    public class JwtService:IJwtService
    {
        private readonly IOptions<MozOptions> _mozOptions;
        private readonly IEncryptionService _encryptionService;

        public JwtService(IOptions<MozOptions> mozOptions, IEncryptionService encryptionService)
        {
            _mozOptions = mozOptions;
            _encryptionService = encryptionService;
            ExpireDateTime = DateTime.Now.AddDays(90);
        }
        
        public DateTime ExpireDateTime { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberUId"></param>
        /// <returns></returns>
        public string GenerateJwtToken(string memberUId)
        {
            if (string.IsNullOrEmpty(memberUId))
                throw new FatalException("member UId 不能为空");

            var claims = new[] 
            {
                new Claim(JwtRegisteredClaimNames.Jti,memberUId),
                new Claim(JwtRegisteredClaimNames.Exp,ExpireDateTime.ToUniversalTime().ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_mozOptions.Value.EncryptKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                "https://ut32.com",
                "moz_application",
                claims,
                expires: ExpireDateTime,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GenerateRefreshToken(string memberUId)
        {
            var guid = Guid.NewGuid().ToString("N");
            var finalText = $"{DateTime.Now.ToUnixTime()}|{memberUId}|{guid}";
            return _encryptionService.EncryptText(finalText);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidIssuer = "https://ut32.com",
                ValidAudience = "moz_application",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_mozOptions.Value.EncryptKey))
            };
        }
    }
}