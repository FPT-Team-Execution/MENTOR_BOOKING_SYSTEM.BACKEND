using System.ComponentModel.DataAnnotations;

namespace MBS.Application.Models.User;

public class SignInRequestModel
{
    [Required] public required string Email { get; set; }

    [Required] public required string Password { get; set; }
}