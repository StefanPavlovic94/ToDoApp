using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using UserManagement.Core.Abstractions;
using UserManagement.Core.Enums;

namespace UserManagement.WebApi.Controllers
{
    public class BaseController : ApiController
    {
        private readonly IJwtService _jwtService;

        public int UserId { get; set; }

        public BaseController(IJwtService jwtService)
        {
            this._jwtService = jwtService;
        }

        public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            var authorization = controllerContext.Request.Headers.Authorization;

            if (authorization != null)
            {
                var jwtInfo = _jwtService.ReadJwtToken(authorization.Parameter, JwtTokenType.AccessToken);

                if (jwtInfo.IsValidJwt == false)
                {
                    return base.ExecuteAsync(controllerContext, cancellationToken);
                }

                var userId = jwtInfo.Claims.First(c => c.Key == CustomClaimType.UserId).Value;

                var parseCompleted = int.TryParse(userId, out int result);

                if (parseCompleted)
                {
                    this.UserId = result;
                }
            }

            return base.ExecuteAsync(controllerContext, cancellationToken);
        }
    }
}
