using MBS.Core.Entities;

namespace MBS.Application.Models.User;

public class GetMentorRequestModel
{
    public required string Id { get; set; }
}

public class GetMentorResponseModel
{
    public string Id { get; set; }
    public string? Industry { get; set; } = default;
    public int ConsumePoint { get; set; } = default;
    public IEnumerable<Major> Major { get; set; }
<<<<<<< HEAD

=======
    
>>>>>>> parent of 4cb5763 (merge query to test api with data)
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
    public string? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}