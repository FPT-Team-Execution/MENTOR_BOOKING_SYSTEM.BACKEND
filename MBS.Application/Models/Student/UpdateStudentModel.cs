namespace MBS.Application.Models.Student;

public class UpdateStudentRequestModel
{
    public string Id { get; set; }
    public string? University { get; set; }
    public int WalletPoint { get; set; }

    public string MajorId { get; set; }

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

public class UpdateStudentResponseModel
{
    public bool Succeed { get; set; }
}