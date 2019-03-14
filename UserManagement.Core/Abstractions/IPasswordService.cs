using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Model;

namespace UserManagement.Core.Abstractions
{
    public interface IPasswordService
    {
        string Hash(string password, string salt);
        string GenerateSalt();
        bool IsValidPassword(string password, string passwordHash, string passwordSalt);
    }
}
