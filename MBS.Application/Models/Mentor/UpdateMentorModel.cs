namespace MBS.Application.Models.Mentor;

public class UpdateMentorRequestModel
{
    public string Id { get; set; }
    public string? Industry { get; set; } = default;
    public int ConsumePoint { get; set; } = default;

    //inheritant
    public string FullName { get; set; }
    public string? AvatarUrl { get; set; }
    public string Gender { get; set; }
    public DateTime? Birthday { get; set; }

    // IdentityUser Properties
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool EmailConfirmed { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public bool LockoutEnabled { get; set; }
}

public class UpdateMentorResponseModel
{
    public bool Succeed { get; set; }
}