using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Abstractions;

namespace UserManagement.Core.Implementations
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IPersistance _persistance;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;

        public AuthorizationService(
            IPersistance persistance,
            IPasswordService passwordService,
            IJwtService jwtService)
        {
            this._persistance = persistance;
            this._jwtService = jwtService;
            this._passwordService = passwordService;
        }

        public string Login(string email, string password, string passwordRetype)
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(passwordRetype))
            {
                return null;
            }

            if (password != passwordRetype)
            {
                return null;
            }

            var passwordModel = this._persistance.AuthorizationRepository.GetPasswordInfo(email);

            if (passwordModel == null)
            {
                return null;
            }

            if (!this._passwordService.IsValidPassword(password, passwordModel.Hash))
            {
                return null;
            }

            return _jwtService.GenerateJwt(passwordModel.UserId);
        }
    }
}
