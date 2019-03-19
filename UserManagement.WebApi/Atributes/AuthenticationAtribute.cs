using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using UserManagement.Core.Abstractions;
using UserManagement.Core.Enums;
using UserManagement.WebApi.Models;

namespace UserManagement.WebApi.Atributes
{
    public class AuthenticationAtribute : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple { get { return false; } }

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            try
            {               
                HttpRequestMessage request = context.Request;
                AuthenticationHeaderValue authorization = request.Headers.Authorization;

                if (authorization == null)
                {
                    context.ErrorResult = new AuthenticationFailureResult("Missing autorization header", request);
                    return Task.FromResult(context);
                }
                if (authorization.Scheme != "Bearer")
                {
                    context.ErrorResult = new AuthenticationFailureResult("Invalid autorization scheme", request);
                    return Task.FromResult(context);
                }
                if (string.IsNullOrEmpty(authorization.Parameter))
                {
                    context.ErrorResult = new AuthenticationFailureResult("Missing Token", request);
                    return Task.FromResult(context);
                }

                IJwtService _jwtTokenService = context.ActionContext.ControllerContext.Configuration
                                 .DependencyResolver.GetService(typeof(IJwtService)) as IJwtService;

                bool isValidToken = _jwtTokenService.ValidateJwtToken(authorization.Parameter, JwtTokenType.AccessToken);

                if (!isValidToken)
                {
                    context.ErrorResult = new AuthenticationFailureResult("Invalid Token", request);
                }

                return Task.FromResult(context);
            }
            catch (Exception ex)
            {
                context.ErrorResult = new AuthenticationFailureResult("Exception: \n" + ex.Message, context.Request);
                return Task.FromResult(context);
            }
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}