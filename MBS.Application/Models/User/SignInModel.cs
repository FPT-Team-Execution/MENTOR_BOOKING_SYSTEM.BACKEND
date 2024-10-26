using System.ComponentModel.DataAnnotations;
using MBS.Application.Models.General;

namespace MBS.Application.Models.User;

public class SignInRequestModel
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }

    [Required] 
    [DataType(DataType.Password)]
    public required string Password { get; set; }
}

public class SignInResponseModel
{
    public JwtModel JwtToken { get; set; }
}