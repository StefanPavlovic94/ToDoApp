﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Core.ResponseModel
{
    public class AuthenticationResponse
    {
        public bool IsAuthenticated { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
