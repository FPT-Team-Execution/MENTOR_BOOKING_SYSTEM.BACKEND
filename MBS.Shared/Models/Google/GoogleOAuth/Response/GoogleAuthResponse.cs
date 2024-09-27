﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Shared.Models.Google
{
    public class GoogleAuthResponse
    {
        public string Name { get; set; }
        public string GivenName { get; set; }
        public string Email { get; set; }
        public string GoogleRefreshToken { get; set; }
        public string GoogleAccessToken { get; set; }
        public string ProviderKey { get; set; }
        public string LoginProvider { get; set; }
    }
}