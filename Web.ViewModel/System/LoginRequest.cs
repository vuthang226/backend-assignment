﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Web.ViewModel.System
{
    public class LoginRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}