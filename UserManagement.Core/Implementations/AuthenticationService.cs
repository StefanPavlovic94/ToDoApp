using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Abstractions;
using UserManagement.Core.Enums;
using UserManagement.Core.Model;

namespace UserManagement.Core.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IPersistance _persistance;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;
        private readonly IAuthenticationValidationService _authenticationValidation;

        public AuthenticationService(
            IPersistance persistance,
            IPasswordService passwordService,
            IJwtService jwtService,
            IAuthenticationValidationService authenticationValidation)
        {
            this._persistance = persistance;
            this._jwtService = jwtService;
            this._passwordService = passwordService;
            this._authenticationValidation = authenticationValidation;
        }

        public AuthenticationResponse Login(string email, string password)
        {
            var authenticationResponse = new AuthenticationResponse() { IsAuthenticated = false };

            var validationResult = _authenticationValidation.ValidateLogin(email, password);

            if (!validationResult)
            {
                return authenticationResponse;
            }

            var passwordModel = this._persistance.AuthorizationRepository.GetPasswordInfo(email);

            if (passwordModel == null)
            {
                return authenticationResponse;
            }

            if (!this._passwordService.IsValidPassword(password, passwordModel.Hash, passwordModel.Salt))
            {
                return authenticationResponse;
            }

            return _jwtService.GenerateJwtTokens(passwordModel.UserId);
        }

        public AuthenticationResponse RefreshToken(string refreshToken)
        {
            var isRefreshTokenValid = this._jwtService.ValidateJwtToken(refreshToken, JwtTokenType.RefreshToken);

            if (!isRefreshTokenValid)
            {
                return new AuthenticationResponse()
                {
                    IsAuthenticated = false,
                    AccessToken = null,
                    RefreshToken = null
                };
            }

            return this._jwtService.GenerateJwtTokens(0);
        }       
    }
}
