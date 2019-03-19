using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserManagement.WebApi.Utilities
{
    public class AutoMapperConfiguration
    {
        public MapperConfiguration Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserMapperProfile>();
            });

            return config;
        }
    }
}