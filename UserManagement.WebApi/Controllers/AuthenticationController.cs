using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UserManagement.Core.Abstractions;
using UserManagement.Core.ResponseModel;
using UserManagement.WebApi.ViewModels;

namespace UserManagement.WebApi.Controllers
{
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(
            IAuthenticationService authorizationService, 
            IJwtService jwtService) 
            : base(jwtService)
        {
            this._authenticationService = authorizationService;
        }

        public AuthenticationResponse Authenticate(AuthenticateViewModel model)
        {
            return this._authenticationService.Login(model.Email, model.Password);
        }

        public AuthenticationResponse RefreshToken(string refreshToken)
        {
            return this._authenticationService.RefreshToken(refreshToken);
        }
    }
}
