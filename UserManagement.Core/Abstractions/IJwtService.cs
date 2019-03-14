using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Core.Abstractions
{
    public interface IJwtService
    {
        string GenerateJwt(int userId);
    }
}
