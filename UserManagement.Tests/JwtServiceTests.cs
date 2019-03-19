using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserManagement.Core.Abstractions;
using UserManagement.Core.Enums;
using UserManagement.Implementation.Services;

namespace UserManagement.Tests
{
    [TestClass]
    public class JwtServiceTests
    {
        private readonly IJwtService _jwtService;

        public JwtServiceTests()
        {
            //todo: Move this secrets to unit test config file
            string accessTokenSecret = "OSrgP0jDTDRdr/7haQGP4F4GOMFZgRp0Q8XGM7HtzEtNZtRDRv1JPSpmmWxfUD4r+mik+LVgcx7Dht7oIOcJNy5rPQIglvK8WWLrhqNJ6FX9Zz8VjHINj4wWVdnE4+QMkMV6/1mEwWyb5dj0IIC0d0pCZSy4V3kdjOrmZYmUqt0=";
            string refreshTokenSecret = "1s0+tq5tPMaKXlyw8SLZUHzd5lXpI4PufpzTe03TJ8CSSccRrCdjR3npC3njqIk/nIYv2k3XpHgu3O+4Tif2ojzRSHf9xco74HKvUYkYq1d7eobViwKqT+Y+5pz/nmCOAL0OslSUO85UfD06FGwQyqJJsmpHmGNAordsmyz5yDM=";

            this._jwtService = new JwtService(accessTokenSecret, refreshTokenSecret);
        }

        [TestMethod]
        public void TestJwtTokensCreationFlow()
        {
            var token = this._jwtService.GenerateJwtTokens(1);
            Assert.AreNotEqual(null, token);
        }

        [TestMethod]
        public void TestAccessTokenCreation()
        {
            var token = this._jwtService.GenerateJwtTokens(1);

            Assert.AreNotEqual(null, token.AccessToken);
            Assert.AreNotEqual("", token.AccessToken);       
        }

        [TestMethod]
        public void TestRefreshTokenCreation()
        {
            var token = this._jwtService.GenerateJwtTokens(1);

            Assert.AreNotEqual(null, token.RefreshToken);
            Assert.AreNotEqual("", token.RefreshToken); 
        }

        [TestMethod]
        public void TestAccessTokenValidation()
        {
            var tokens = this._jwtService.GenerateJwtTokens(1);

            var isValidToken = this._jwtService.ValidateJwtToken(tokens.AccessToken, JwtTokenType.AccessToken);

            Assert.AreEqual(true, isValidToken);
        }

        [TestMethod]
        public void TestRefreshTokenValidation()
        {
            var tokens = this._jwtService.GenerateJwtTokens(1);

            var isValidToken = this._jwtService.ValidateJwtToken(tokens.RefreshToken, JwtTokenType.RefreshToken);

            Assert.AreEqual(true, isValidToken);
        }

        [TestMethod]
        public void TestAccessTokenValidationWrongSecret()
        {
            var tokens = this._jwtService.GenerateJwtTokens(1);

            var isValidToken = this._jwtService.ValidateJwtToken(tokens.AccessToken, JwtTokenType.RefreshToken);

            Assert.AreNotEqual(true, isValidToken);
        }

        [TestMethod]
        public void TestRefreshTokenValidationWrongSecret()
        {
            var tokens = this._jwtService.GenerateJwtTokens(1);

            var isValidToken = this._jwtService.ValidateJwtToken(tokens.RefreshToken, JwtTokenType.AccessToken);

            Assert.AreNotEqual(true, isValidToken);
        }
    }
}
