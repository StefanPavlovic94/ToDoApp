using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Core.Model
{
    public class JwtInfo
    {
        public bool IsValidJwt { get; set; }
        public Dictionary<CustomClaim, string> Claims { get; set; }
    }

    public static class Extensions
    {
        public static JwtInfo WithClaim(this JwtInfo jwtInfo, CustomClaim claim, string value, bool validJwt)
        {
            if (jwtInfo == null)
            {
                throw new NullReferenceException("JwtInfo object is null");
            }

            if (jwtInfo.Claims == null)
            {
                jwtInfo.Claims = new Dictionary<CustomClaim, string>();
            }

            jwtInfo.IsValidJwt = validJwt;
            jwtInfo.Claims.Add(claim, value);

            return jwtInfo;
        }
    }

    public enum CustomClaim
    {
        UserId = 1
    }

    public enum JwtTokenType
    {
        AccessToken = 1,
        RefreshToken = 2
    }
}
