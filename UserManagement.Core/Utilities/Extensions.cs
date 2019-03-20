using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Enums;
using UserManagement.Core.ResponseModel;

namespace UserManagement.Core.Utilities
{
    public static class Extensions
    {
        public static AuthenticationInfo WithClaim(this AuthenticationInfo jwtInfo, CustomClaimType claim, string value, bool validJwt)
        {
            if (jwtInfo == null)
            {
                throw new NullReferenceException("JwtInfo object is null");
            }

            if (jwtInfo.Claims == null)
            {
                jwtInfo.Claims = new Dictionary<CustomClaimType, string>();
            }

            jwtInfo.IsValidJwt = validJwt;
            jwtInfo.Claims.Add(claim, value);

            return jwtInfo;
        }
    }
}
