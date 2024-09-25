namespace MBS.Application.Models.User;

public class GetMentorOwnProfileRequestModel
{
    public required Guid MentorId { get; set; }
}

public class GetMentorOwnProfileResponseModel
{
    public required string FullName { get; set; }
    public string? AvatarUrl { get; set; }
    public required string Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public string? Industry { get; set; }
    public int ConsumePoint { get; set; }
}