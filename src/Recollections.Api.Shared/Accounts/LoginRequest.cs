﻿using Neptuo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Recollections.Accounts
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginRequest()
        { }

        public LoginRequest(string userName, string password)
        {
            Ensure.NotNull(userName, "userName");
            Ensure.NotNull(password, "password");
            UserName = userName;
            Password = password;
        }
    }
}
