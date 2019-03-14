﻿using System.Linq;
using UserManagement.Core.Abstractions;
using UserManagement.Core.Model;

namespace UserManagement.Persistance.Implementations
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private UserContext userContext { get; set; }

        public AuthorizationRepository(UserContext userContext)
        {
            this.userContext = userContext;
        }

        public void CreatePassword(Password password)
        {
            this.userContext.Passwords.Add(password);
        }

        public Password GetPasswordInfo(string email)
        {
            return this.userContext.Passwords.SingleOrDefault(p => p.User.Email == email);
        }
    }
}