using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserManagement.Core.Abstractions;
using UserManagement.Core.DomainModel;
using UserManagement.Implementation.Services;

namespace UserManagement.Tests
{
    [TestClass]
    public class UserValidationServiceTests
    {
        IUserValidationService _userValidationService;

        public UserValidationServiceTests()
        {
            this._userValidationService = new UserValidationService();
        }

        [TestMethod]
        public void TestValidUserValidationForEditing()
        {
            var validUser = new User()
            {
                Email = "john.doe@email.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var isUserValid = this._userValidationService.ValidateEditUser(validUser);

            Assert.AreEqual(true, isUserValid);
        }

        [TestMethod]
        public void TestEditUserValidationWithNotValidEmail()
        {
            var validUser = new User()
            {
                Email = "asdasdasdasdasdasd",
                FirstName = "John",
                LastName = "Doe"
            };

            var isUserValid = this._userValidationService.ValidateEditUser(validUser);

            Assert.AreEqual(true, isUserValid);
        }

        [TestMethod]
        public void TestEditUserValidationWithNotValidFirstName()
        {
            var validUser = new User()
            {
                Email = "john.doe@email.com",
                FirstName = "",
                LastName = "Doe"
            };

            var isUserValid = this._userValidationService.ValidateEditUser(validUser);

            Assert.AreEqual(false, isUserValid);
        }

        [TestMethod]
        public void TestEditUserValidationWithNotValidLastName()
        {
            var validUser = new User()
            {
                Email = "john.doe@email.com",
                FirstName = "John",
                LastName = ""
            };

            var isUserValid = this._userValidationService.ValidateEditUser(validUser);

            Assert.AreEqual(false, isUserValid);
        }

        [TestMethod]
        public void TestEditUserValidationWithUserNull()
        {
            var isUserValid = this._userValidationService.ValidateEditUser(null);

            Assert.AreEqual(false, isUserValid);
        }

        [TestMethod]
        public void TestValidUserValidationForRegistration()
        {
            var validUser = new User()
            {
                Email = "john.doe@email.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var isUserValid = this._userValidationService.ValidateRegisterUserParams(validUser, "password1");

            Assert.AreEqual(true, isUserValid);
        }

        [TestMethod]
        public void TestUserValidationForRegistrationWithNotValidEmail()
        {
            var validUser = new User()
            {
                Email = "asdasdasdasdasd",
                FirstName = "John",
                LastName = "Doe"
            };

            var isUserValid = this._userValidationService.ValidateRegisterUserParams(validUser, "password1");

            Assert.AreEqual(false, isUserValid);
        }

        [TestMethod]
        public void TestUserValidationForRegistrationWithNotValidFirstName()
        {
            var validUser = new User()
            {
                Email = "john.doe@email.com",
                FirstName = "",
                LastName = "Doe"
            };

            var isUserValid = this._userValidationService.ValidateRegisterUserParams(validUser, "password1");

            Assert.AreEqual(false, isUserValid);
        }

        [TestMethod]
        public void TestUserValidationForRegistrationWithNotValidLastName()
        {
            var validUser = new User()
            {
                Email = "john.doe@email.com",
                FirstName = "John",
                LastName = ""
            };

            var isUserValid = this._userValidationService.ValidateRegisterUserParams(validUser, "password1");

            Assert.AreEqual(false, isUserValid);
        }

        [TestMethod]
        public void TestUserValidationForRegistrationWithNotValidPassword()
        {
            var validUser = new User()
            {
                Email = "john.doe@email.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var isUserValid = this._userValidationService.ValidateRegisterUserParams(validUser, "");

            Assert.AreEqual(false, isUserValid);
        }

        [TestMethod]
        public void TestUserValidationForRegistrationWithUserNull()
        {
            var isUserValid = this._userValidationService.ValidateRegisterUserParams(null, "password1");

            Assert.AreEqual(false, isUserValid);
        }
    }
}
