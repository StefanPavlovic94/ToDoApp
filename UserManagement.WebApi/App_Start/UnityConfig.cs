using AutoMapper;
using System;
using System.Web.Http;
using Unity;
using Unity.AspNet.WebApi;
using UserManagement.Core.Abstractions;
using UserManagement.Core.Implementations;
using UserManagement.Implementation.Services;
using UserManagement.Persistance.Implementations;
using UserManagement.WebApi.Utilities;

namespace UserManagement.WebApi
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IUserValidationService, UserValidationService>();
            container.RegisterType<IPasswordService, PasswordService>();
            container.RegisterType<IJwtService, JwtService>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IPasswordRepository, PasswordRepository>();
            container.RegisterType<IPersistance, Persistance.Implementations.Persistance>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IAuthenticationService, AuthenticationService>();

            var config = new AutoMapperConfiguration().Configure();
            IMapper mapper = config.CreateMapper();

            container.RegisterInstance(mapper);

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}