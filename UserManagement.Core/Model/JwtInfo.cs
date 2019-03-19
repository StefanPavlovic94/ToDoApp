using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Enums;

namespace UserManagement.Core.Model
{
    public class JwtInfo
    {
        public bool IsValidJwt { get; set; }
        public Dictionary<CustomClaimType, string> Claims { get; set; }
    }

    public static class Extensions
    {
        public static JwtInfo WithClaim(this JwtInfo jwtInfo, CustomClaimType claim, string value, bool validJwt)
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
