using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using UserManagement.Core.Abstractions;
using UserManagement.WebApi.Atributes;
using UserManagement.WebApi.Models;
using UserManagement.WebApi.ViewModels;

namespace UserManagement.WebApi.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(
            IUserService userService, 
            IMapper mapper,
            IJwtService jwtService) 
            : base(jwtService)
        {
            this._userService = userService;
            this._mapper = mapper;
        }

        [HttpGet]
        public JsonResult<UserViewModel> Get(int id)
        {
            var user = this._userService.GetUser(id);
            var userViewModel = this._mapper.Map<UserViewModel>(user);

            return Json(userViewModel);
        }

        [HttpPost]
        public JsonResult<UserViewModel> Register(CreateUserViewModel userViewModel)
        {
            var user = this._mapper.Map<Core.Model.User>(userViewModel);
            var createdUser = this._userService.Register(user, userViewModel.Password);

            var createdUserViewModel = this._mapper.Map<UserViewModel>(createdUser);
            return Json(createdUserViewModel);
        }

        [HttpPut]
        [AuthenticationAtribute]
        public JsonResult<UserViewModel> Edit(CreateUserViewModel userViewModel)
        {
            var user = this._mapper.Map<Core.Model.User>(userViewModel);
            var editedUser = this._userService.EditUser(user);

            var editedUserViewModel = this._mapper.Map<UserViewModel>(editedUser);
            return Json(editedUserViewModel);
        }

        [HttpDelete]
        [AuthenticationAtribute]
        public JsonResult<UserViewModel> Delete(int id)
        {
            var user = this._userService.DeleteUser(id);
            var userViewModel = this._mapper.Map<UserViewModel>(user);

            return Json(userViewModel);
        }
    }
}
