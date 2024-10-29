

namespace MBS.Shared.Models.Google.GoogleOAuth.Response
{
    public class GoogleAuthResponse : GoogleResponse
    {
        // public string Name { get; set; }
        // public string GivenName { get; set; }
        // public string Email { get; set; }
        // public string GoogleRefreshToken { get; set; }
        // public string GoogleAccessToken { get; set; }
        public string ProviderKey { get; set; }
        public string LoginProvider { get; set; }
    }
    public class GoogleAuthErrorResponse : GoogleResponse
    {
        public string error { get; set; }
        public string error_description { get; set; }
    }
}
