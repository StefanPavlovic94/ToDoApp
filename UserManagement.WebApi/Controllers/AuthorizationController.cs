using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UserManagement.Core.Abstractions;
using UserManagement.WebApi.Models;

namespace UserManagement.WebApi.Controllers
{
    public class AuthorizationController : ApiController
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;

        public AuthorizationController(
            IAuthorizationService authorizationService,
            IMapper mapper)
        {
            this._authorizationService = authorizationService;
            this._mapper = mapper;
        }

        public string Authorize(AuthorizeViewModel model)
        {
            return this._authorizationService.Login(model.Email, model.Password);
        }
    }
}
