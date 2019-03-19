using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Enums;
using UserManagement.Core.Model;

namespace UserManagement.Core.Abstractions
{
    public interface IJwtService
    {
        AuthenticationResponse GenerateJwtTokens(int userId);
        JwtInfo ReadJwtToken(string token, JwtTokenType tokenType);
        bool ValidateJwtToken(string token, JwtTokenType tokenType);
    }
}
