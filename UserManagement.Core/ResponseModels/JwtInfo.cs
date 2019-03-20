using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Enums;

namespace UserManagement.Core.ResponseModel
{
    public class AuthenticationInfo
    {
        public bool IsValidJwt { get; set; }
        public Dictionary<CustomClaimType, string> Claims { get; set; }
    }   
}
