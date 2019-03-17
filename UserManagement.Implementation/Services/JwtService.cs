using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Abstractions;
using UserManagement.Core.Model;

namespace UserManagement.Implementation.Services
{
    public class JwtService : IJwtService
    {
        private readonly string _accessTokenSecret;
        private readonly string _refreshTokenSecret;

        public JwtService(string accessTokenSecret, string refreshTokenSecret)
        {
            this._accessTokenSecret = accessTokenSecret;
            this._refreshTokenSecret = refreshTokenSecret;
        }

        public AuthenticationResponse GenerateJwtTokens(int userId)
        {
            var accessTokenBase64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(this._accessTokenSecret));
            var refreshTokenBase64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(this._refreshTokenSecret));

            byte[] accessTokenKey = Convert.FromBase64String(accessTokenBase64String);
            SymmetricSecurityKey accessTokenSecurityKey = new SymmetricSecurityKey(accessTokenKey);

            byte[] refreshTokenKey = Convert.FromBase64String(refreshTokenBase64String);
            SymmetricSecurityKey refreshTokenSecurityKey = new SymmetricSecurityKey(refreshTokenKey);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] {            
                new Claim(CustomClaim.UserId.ToString(), userId.ToString())
            });

            SecurityTokenDescriptor accessTokenDescriptor = new SecurityTokenDescriptor
            {
                      Subject = claimsIdentity,                      
                      Expires = DateTime.UtcNow.AddMinutes(1),
                      SigningCredentials = new SigningCredentials(accessTokenSecurityKey,SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityTokenDescriptor refreshTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(refreshTokenSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            JwtSecurityToken accessSecurityToken = handler.CreateJwtSecurityToken(accessTokenDescriptor);
            JwtSecurityToken refreshSecurityToken = handler.CreateJwtSecurityToken(refreshTokenDescriptor);

            return new AuthenticationResponse()
            {
                IsAuthenticated = true,
                AccessToken = handler.WriteToken(accessSecurityToken),
                RefreshToken = handler.WriteToken(refreshSecurityToken)
            };
        }

        public bool ValidateJwtToken(string token, JwtTokenType tokenType)
        {
            try
            {
                var secretKey = GetSecretKey(tokenType);

                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };

                IPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public JwtInfo ReadJwtToken(string token, JwtTokenType tokenType)
        {
            var isValidJwt = ValidateJwtToken(token, tokenType);

            if (!isValidJwt)
            {
                return new JwtInfo() { IsValidJwt = false };
            }

            var secretKey = GetSecretKey(tokenType);

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.ReadToken(token) as JwtSecurityToken;

            var userId = securityToken.Claims
                .First(c => c.ValueType == CustomClaim.UserId.ToString())
                .Value;

            return new JwtInfo()
                .WithClaim(CustomClaim.UserId, userId.ToString(), validJwt: true);
        }

        private string GetSecretKey(JwtTokenType tokenType)
        {
            switch (tokenType)
            {
                case JwtTokenType.AccessToken:
                    return _accessTokenSecret;

                case JwtTokenType.RefreshToken:
                    return _refreshTokenSecret;

                default:
                    throw new Exception($"Token type {tokenType} dont have secret key");
            }

        }
    }
}
