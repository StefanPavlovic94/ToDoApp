using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserManagement.Core.Abstractions;
using UserManagement.Core.Implementations;
using UserManagement.Core.DomainModel;
using UserManagement.Implementation.Services;
using UserManagement.Persistance.Implementations;

namespace UserManagement.Tests
{
    [TestClass]
    public class AuthenticationServiceTests
    {
        IAuthenticationService _authenticationService;
        IPasswordService _passwordService;

        string validEmail = "john.doe@email.com";
        string validPassword = "password1";

        public AuthenticationServiceTests()
        {
            string accessTokenSecret = ConfigurationManager.AppSettings["AccessTokenSecret"];
            string refreshTokenSecret = ConfigurationManager.AppSettings["RefreshTokenSecret"];

            IJwtService jwtService = new JwtService(accessTokenSecret, refreshTokenSecret);
            this._passwordService = new PasswordService();

            UserContext inMemoryContext = UserContext.AsInMemoryDatabase();
            this.AddMockupUsersToDatabase(inMemoryContext);

            IPersistance persistance = new Persistance.Implementations.Persistance(inMemoryContext);
            IAuthenticationValidationService authenticationValidation = new AuthenticationValidationService();

            this._authenticationService = new AuthenticationService(persistance, _passwordService, jwtService, authenticationValidation);
        }

        private void AddMockupUsersToDatabase(UserContext userContext)
        {
            var user = new User()
            {
                Email = this.validEmail,
                FirstName = "John",
                LastName = "Doe"
            };

            userContext.Users.Add(user);
            userContext.SaveChanges();

            var salt = this._passwordService.GenerateSalt();
            var hash = this._passwordService.Hash(this.validPassword, salt);

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
        public void TestLoginWithValidEmailAndPassword()
        {
            var authenticationResponse = this._authenticationService.Login(this.validEmail, this.validPassword);

            Assert.AreEqual(authenticationResponse.IsAuthenticated, true);
            Assert.AreNotEqual(null, authenticationResponse.AccessToken);
            Assert.AreNotEqual(null, authenticationResponse.RefreshToken);
        }

        [TestMethod]
        public void TestLoginWithInvalidPassword()
        {
            var authenticationResponse = this._authenticationService.Login(this.validEmail, "pswrd123");
            Assert.AreEqual(authenticationResponse.IsAuthenticated, false);
            Assert.AreEqual(null, authenticationResponse.AccessToken);
            Assert.AreEqual(null, authenticationResponse.RefreshToken);
        }

        [TestMethod]
        public void TestLoginWithInvalidEmail()
        {
            var authenticationResponse = this._authenticationService.Login("johan.doe@email.com", this.validPassword);
            Assert.AreEqual(authenticationResponse.IsAuthenticated, false);
            Assert.AreEqual(null, authenticationResponse.AccessToken);
            Assert.AreEqual(null, authenticationResponse.RefreshToken);
        }
    }
}
