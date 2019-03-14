using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Abstractions;
using UserManagement.Core.Model;

namespace UserManagement.Implementation
{
    public class PasswordService : IPasswordService
    {
        public string Hash(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public bool IsValidPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        private string GenerateSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt();
        }

        public Password CreatePasswordModel(string password, int userId)
        {
            var salt = this.GenerateSalt();
            var hash = this.Hash(password, salt);

            var passwordModel = new Password()
            {
                UserId = userId,
                Salt = salt,
                Hash = hash
            };

            return passwordModel;
        }

    }
}
