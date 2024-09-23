namespace MBS.Application.Models.User;

public class SignInResponseModel
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}