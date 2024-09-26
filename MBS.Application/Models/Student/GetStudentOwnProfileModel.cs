namespace MBS.Application.Models.User;

public class GetStudentOwnProfileRequestModel
{
    public required Guid UserId { get; set; }
}

public class GetStudentOwnProfileResponseModel
{
    public required string FullName { get; set; }
    public string? AvatarUrl { get; set; }
    public required string Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public string? University { get; set; }
    public int WalletPoint { get; set; }
}