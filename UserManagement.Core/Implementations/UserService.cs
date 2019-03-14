using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Abstractions;
using UserManagement.Core.Model;

namespace UserManagement.Core.Implementations
{
    public class UserService : IUserService
    {
        private readonly IPersistance _persistance;
        private readonly IPasswordService _passwordService;
        private readonly IUserValidationService _userValidationService;

        public UserService(
            IPersistance persistance,
            IPasswordService passwordService, 
            IUserValidationService userValidationService)
        {
            this._persistance = persistance;
            this._passwordService = passwordService;
            this._userValidationService = userValidationService;
        }

        public User GetUser(int userId)
        {
            return this._persistance.UserRepository.GetUser(userId);
        }

        public User EditUser(User user)
        {
            return this._persistance.UserRepository.EditUser(user);
        }

        public User DeleteUser(int userId)
        {
            return this._persistance.UserRepository.DeleteUser(userId);
        }

        public User Register(User user, string password)
        {
            var registerUserParamsValid = this._userValidationService.ValidateRegisterUserParams(user, password);

            if (!registerUserParamsValid)
            {
                return null;
            }

            User createdUser = null;

            using (this._persistance.BeginTransaction())
            {
                createdUser = this._persistance.UserRepository.CreateUser(user);
                this._persistance.SaveChanges();

                Password passwordModel = this._passwordService.CreatePasswordModel(password, createdUser.Id); //todo: MOve this in password repository

                this._persistance.AuthorizationRepository.CreatePassword(passwordModel);

                this._persistance.TransactionCommit();
            }

            return createdUser;
        }
    }   
}
