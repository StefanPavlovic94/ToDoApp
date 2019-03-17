using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserManagement.Core.Abstractions;
using UserManagement.Implementation.Services;

namespace UserManagement.Tests
{
    [TestClass]
    public class PasswordServiceTests
    {
        private readonly IPasswordService _passwordService;

        public PasswordServiceTests()
        {
            this._passwordService = new PasswordService();
        }

        [TestMethod]
        public void TestValidPasswordFlow()
        {
            var password = "password1";
            var salt = this._passwordService.GenerateSalt();

            var passwordHash = this._passwordService.Hash(password, salt);

            var isPasswordValid = this._passwordService.IsValidPassword(password, passwordHash, salt);

            Assert.AreEqual(true, isPasswordValid);
        }

        [TestMethod]
        public void TestInvalidPasswordFlow()
        {
            var password = "password1";
            var invalidPassword = "notPassword1";

            var salt = this._passwordService.GenerateSalt();

            var passwordHash = this._passwordService.Hash(password, salt);

            var isPasswordValid = this._passwordService.IsValidPassword(invalidPassword, passwordHash, salt);

            Assert.AreNotEqual(true, isPasswordValid);
        }       
    }
}
