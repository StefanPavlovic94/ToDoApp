using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserManagement.Core.Abstractions;
using UserManagement.Core.Implementations;
using UserManagement.Core.DomainModel;
using UserManagement.Implementation.Services;
using UserManagement.Persistance.Implementations;

namespace UserManagement.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        public readonly IUserService _userService;

        private const string validEmail = "john.doe@email.com";
        private const string validPassword = "password1";

        public UserServiceTests()
        {
            UserContext inMemoryContext = UserContext.AsInMemoryDatabase();
            IPersistance persistance = new Persistance.Implementations.Persistance(inMemoryContext);
            IPasswordService passwordService = new PasswordService();
            IUserValidationService userValidationService = new UserValidationService();

            this._userService = new UserService(persistance, passwordService, userValidationService);
        }

        [TestMethod]
        public void RegisterValidUser()
        {
            var validUser = new User()
            {
                FirstName = "john",
                LastName = "Doe",
                Email = UserServiceTests.validEmail
            };

            var registeredUser = this._userService.Register(validUser, UserServiceTests.validPassword);

            Assert.AreNotEqual(null, registeredUser);
            Assert.AreEqual(registeredUser.Email, validUser.Email);
            Assert.AreNotEqual(0, registeredUser.Id);
        }

        [TestMethod]
        public void RegisterUserWithNotValidEmail()
        {
            var validUser = new User()
            {
                FirstName = "john",
                LastName = "Doe",
                Email = "johnasdasdasd"
            };

            var registeredUser = this._userService.Register(validUser, UserServiceTests.validPassword);

            Assert.AreEqual(null, registeredUser);
        }
    }
}
