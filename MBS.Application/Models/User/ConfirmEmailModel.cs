namespace MBS.Application.Models.User;

public class ConfirmEmailRequestModel
{
    public required string Email { get; set; }
    public required string Token { get; set; }
}

public class ConfirmEmailResponseModel
{
    public bool Confirmed { get; set; }
}