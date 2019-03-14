using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UserManagement.Core.Abstractions;
using UserManagement.Core.Model;
using UserManagement.WebApi.Models;

namespace UserManagement.WebApi.Controllers
{
    public class AuthenticationController : ApiController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public AuthenticationController(IAuthenticationService authorizationService)
        {
            this._authenticationService = authorizationService;
        }

        public AuthenticationResponse Authenticate(AuthorizeViewModel model)
        {
            return this._authenticationService.Login(model.Email, model.Password);
        }

        public AuthenticationResponse RefreshToken(string refreshToken)
        {
            return this._authenticationService.RefreshToken(refreshToken);
        }
    }
}
