namespace MBS.Application.Models.General;

public class JwtModel
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}