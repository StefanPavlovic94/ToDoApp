using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Abstractions;
using UserManagement.Core.Model;

namespace UserManagement.Core.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IPersistance _persistance;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;

        public AuthenticationService(
            IPersistance persistance,
            IPasswordService passwordService,
            IJwtService jwtService)
        {
            this._persistance = persistance;
            this._jwtService = jwtService;
            this._passwordService = passwordService;           
        }

        public AuthenticationResponse Login(string email, string password)
        {
            var authenticationResponse = new AuthenticationResponse() { IsAuthenticated = false };

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
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
            //validate refresh token
            return this._jwtService.GenerateJwtTokens(0);
        }
    }
}
