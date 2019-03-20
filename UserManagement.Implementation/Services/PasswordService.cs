using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Abstractions;
using UserManagement.Core.DomainModel;

namespace UserManagement.Implementation.Services
{
    public class PasswordService : IPasswordService
    {
        public string Hash(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public bool IsValidPassword(string password, string passwordHash, string passwordSalt)
        {
            return string.Equals(Hash(password, passwordSalt), passwordHash);
        }

        public string GenerateSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt();
        }       
    }
}
