using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.DomainModel;

namespace UserManagement.Core.Abstractions
{
    public interface IUserService
    {
        User Register(User user, string password);
        User GetUser(int userId);
        User EditUser(User user);
        User DeleteUser(int userId);
    }
}
