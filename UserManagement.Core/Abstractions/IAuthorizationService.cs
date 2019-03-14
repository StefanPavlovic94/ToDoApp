using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Core.Abstractions
{
    public interface IAuthorizationService
    {
        string Login(string email, string password, string passwordRetype);
    }
}
