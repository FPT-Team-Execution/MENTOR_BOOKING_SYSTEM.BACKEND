using MBS.Application.Models.General;

namespace MBS.Application.Models.User;

public class GetRefreshTokenRequestModel
{
    public required string RefreshToken { get; set; }
}

public class GetRefreshTokenResponseModel
{
    public required JwtModel NewJwtToken { get; set; }
}