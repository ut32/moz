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
        private readonly DateTime _expireDateTime; 

        public JwtService(IOptions<MozOptions> mozOptions, IEncryptionService encryptionService)
        {
            _mozOptions = mozOptions;
            _encryptionService = encryptionService;
            _expireDateTime = DateTime.Now.AddDays(30).ToUniversalTime();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberUId"></param>
        /// <returns></returns>
        public TokenInfo GenerateTokenInfo(string memberUId)
        {
            if (string.IsNullOrEmpty(memberUId))
                throw new FatalException("member uid required");

            var claims = new[] 
            {
                new Claim(JwtRegisteredClaimNames.Jti,memberUId),
                new Claim(JwtRegisteredClaimNames.Exp,_expireDateTime.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_mozOptions.Value.EncryptKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                "https://ut32.com",
                "moz_application",
                claims,
                expires: _expireDateTime,
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenInfo
            {
                JwtToken = tokenString, 
                RefreshToken = GenerateRefreshToken(memberUId), 
                ExpireDateTime = _expireDateTime.ToUnixTime()
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GenerateRefreshToken(string memberUId)
        {
            var guid = Guid.NewGuid().ToString("N");
            var finalText = $"{_mozOptions.Value.EncryptKey}|{memberUId}|{guid}";
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