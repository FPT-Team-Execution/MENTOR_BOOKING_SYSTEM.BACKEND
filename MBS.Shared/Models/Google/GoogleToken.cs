using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Shared.Models.Google
{
    public class GoogleToken
    {
        public string? Access_token { get; set; }
        //public string? Refresh_token { get; set; }
        public DateTime Expires_in { get; set; }
        //public string? Token_type { get; set; }
        //public DateTime GetExpiresInDateTime()
        //{
        //    DateTime now = DateTime.UtcNow;
        //    now = now.AddSeconds(Double.Parse(Expires_in));
        //    return now;
        //}
    }
}
