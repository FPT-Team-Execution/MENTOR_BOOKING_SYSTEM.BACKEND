using MBS.Shared.Models.Google;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Shared.Services.Interfaces
{
    public interface IGoogleService
    {
        Task<GoogleAuthResponse?> AuthenticateGoogleUserAsync(HttpContext context);
        Task<String> GetUserEvents(string accessToken);
    }
}
