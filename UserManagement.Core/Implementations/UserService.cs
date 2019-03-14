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
            var userValidForEdit = this._userValidationService.ValidateEditUser(user);

            if (!userValidForEdit)
            {
                return null;
            }

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

                var salt = this._passwordService.GenerateSalt();
                var hash = this._passwordService.Hash(password, salt);

                var passwordModel = new Password()
                {
                    UserId = createdUser.Id,
                    Salt = salt,
                    Hash = hash
                };

                this._persistance.AuthorizationRepository.CreatePassword(passwordModel);

                this._persistance.TransactionCommit();
            }

            return createdUser;
        }
    }   
}
