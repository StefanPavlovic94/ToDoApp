using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.ResponseModel;

namespace UserManagement.Core.Abstractions
{
    public interface IAuthenticationService
    {
        AuthenticationResponse Login(string email, string password);
        AuthenticationResponse RefreshToken(string refreshToken);
    }
}
