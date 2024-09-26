﻿using MBS.Shared.Models.Google;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Shared.Services.Interfaces
{
    public interface IGoogleAuthenticationService
    {
        public Task<GoogleAuthResponse> AuthenticateGoogleUser(HttpContext context);
    }
}