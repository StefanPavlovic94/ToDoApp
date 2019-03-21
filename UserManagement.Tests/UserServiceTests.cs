using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserManagement.Core.Abstractions;
using UserManagement.Core.Implementations;
using UserManagement.Core.DomainModel;
using UserManagement.Implementation.Services;
using UserManagement.Persistance.Implementations;
using System.Linq;

namespace UserManagement.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        public readonly IUserService _userService;
        public readonly IPasswordService _passwordService;

        private const string validEmail = "john.doe@email.com";
        private const string validPassword = "password1";
        private UserContext inMemoryContext;       

        public UserServiceTests()
        {
            this.inMemoryContext = UserContext.AsInMemoryDatabase();

            IPersistance persistance = new Persistance.Implementations.Persistance(inMemoryContext);
            IUserValidationService userValidationService = new UserValidationService();
            this._passwordService = new PasswordService();

            this._userService = new UserService(persistance, this._passwordService, userValidationService);

            AddMockupUsersToDatabase(this.inMemoryContext);
        }

        private User GetValidUser()
        {
            return new User()
            {
                Email = validEmail,
                FirstName = "John",
                LastName = "Doe"
            };
        }

        private void AddMockupUsersToDatabase(UserContext userContext)
        {
            var user = GetValidUser();
            userContext.Users.Add(user);
            userContext.SaveChanges();


            var salt = this._passwordService.GenerateSalt();
            var hash = this._passwordService.Hash(validPassword, salt);

            var password = new Password()
            {
                Hash = hash,
                Salt = salt,
                UserId = user.Id
            };

            userContext.Passwords.Add(password);

            userContext.SaveChanges();
        }

        [TestMethod]
        public void TestRegisterValidUser()
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
        public void TestRegisterUserWithNotValidEmail()
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

        [TestMethod]
        public void TestEditValidUser()
        {
            var existingUserId = this.inMemoryContext.Users.FirstOrDefault().Id;

            var firstNameForEdit = "Patrick";

            var validUser = GetValidUser();

            validUser.Id = existingUserId;
            validUser.FirstName = firstNameForEdit;

            var editedUser = this._userService.EditUser(validUser);

            Assert.AreNotEqual(null, editedUser);
            Assert.AreEqual(firstNameForEdit, editedUser.FirstName);
        }

        [TestMethod]
        public void TestEditNotValidUsersFirstName()
        {
            var existingUserId = this.inMemoryContext.Users.FirstOrDefault().Id;
            var firstNameForEdit = "";

            var validUser = GetValidUser();
            validUser.Id = existingUserId;
            validUser.FirstName = firstNameForEdit;

            var editedUser = this._userService.EditUser(validUser);

            Assert.AreEqual(null, editedUser);
        }

        [TestMethod]
        public void TestDeleteUser()
        {
            var existingUserId = this.inMemoryContext.Users.FirstOrDefault().Id;

            var userFromDatabase = this._userService.GetUser(existingUserId);

            var deletedUser = this._userService.DeleteUser(userFromDatabase.Id);

            var userExist = inMemoryContext.Users.Any(u => u.Id == userFromDatabase.Id);

            Assert.AreNotEqual(null, deletedUser);
            Assert.AreEqual(false, userExist);
        }
    }
}
