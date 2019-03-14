﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserManagement.Core.Abstractions;
using UserManagement.Implementation;

namespace UserManagement.Tests
{
    [TestClass]
    public class JwtServiceTests
    {
        private readonly IJwtService _jwtService;

        public JwtServiceTests()
        {
            this._jwtService = new JwtService();
        }

        [TestMethod]
        public void TestJwtCreation()
        {
            var token = this._jwtService.GenerateJwt(1);

            Assert.AreNotEqual(null, token);
            Assert.AreNotEqual("", token);
        }
    }
}
