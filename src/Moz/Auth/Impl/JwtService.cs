using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moz.Core.Config;
using Moz.Exceptions;
using Moz.Service.Security;

namespace Moz.Auth.Impl
{
    public class JwtService : IJwtService
    {
        private readonly IOptions<AppConfig> _appConfig;
        private readonly IEncryptionService _encryptionService;
        private readonly DateTime _expireDateTime; 

        public JwtService(IOptions<AppConfig> appConfig, IEncryptionService encryptionService)
        {
            _appConfig = appConfig;
            _encryptionService = encryptionService;
            _expireDateTime = DateTime.Now.AddDays(_appConfig.Value.Token.Expire).ToUniversalTime();
        }
         
        /// <summary>
        /// Generate JWT Token
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfig.Value.AppSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                _appConfig.Value.Token.Issuer,
                _appConfig.Value.Token.Audience,
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
        /// Generate Refresh Token
        /// </summary>
        /// <returns></returns>
        private string GenerateRefreshToken(string memberUId)
        {
            var guid = Guid.NewGuid().ToString("N");
            var finalText = $"{_appConfig.Value.AppSecret}|{memberUId}|{guid}";
            return _encryptionService.EncryptText(finalText,_appConfig.Value.AppSecret);
        }

        /// <summary>
        /// Return TokenValidationParameters
        /// </summary>
        /// <returns></returns>
        public TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidIssuer = _appConfig.Value.Token.Issuer,
                ValidAudience =_appConfig.Value.Token.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfig.Value.AppSecret))
            };
        }
    }
}