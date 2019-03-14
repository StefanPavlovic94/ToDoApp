using AutoMapper;
using Microsoft.Owin;
using Owin;
using UserManagement.WebApi.Utilities;

[assembly: OwinStartup(typeof(UserManagement.WebApi.Startup))]

namespace UserManagement.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

        }
    }
}
