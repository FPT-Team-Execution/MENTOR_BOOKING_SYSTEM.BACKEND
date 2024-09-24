using System.ComponentModel.DataAnnotations;

namespace MBS.Application.Models.User;

public class RegisterMentorRequestModel
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }

    [Required] public required string Password { get; set; }

    [MaxLength(100)] [Required] public required string FullName { get; set; }

    public string? AvatarUrl { get; set; }

    [MaxLength(10)] [Required] public required string Gender { get; set; }

    public string? Industry { get; set; }
}

public class RegisterMentorResponseModel
{
    public required string UserId { get; set; }
}