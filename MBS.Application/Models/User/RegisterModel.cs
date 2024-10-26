using System.ComponentModel.DataAnnotations;
using MBS.Application.ValidationAttributes;
using MBS.Core.Enums;

namespace MBS.Application.Models.User
{
    public class RegisterRequestModel
    {
        [Required(ErrorMessage = "Username is required")]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Password is required")] 
        [DataType(DataType.Password)]
        public required string Password { get; set; }
        [Required(ErrorMessage = "FullName is required")] 
        [MaxLength(100, ErrorMessage = "Full name cannot be longer than 100 characters")]
        public required string FullName { get; set; }

        [MaxLength(10, ErrorMessage = "Username cannot be longer than 10 characters")]
        [Required(ErrorMessage = "Gender is required")] 
        [EnumValidation(typeof(GenderEnum))]
        public required string Gender { get; set; }
        [Required(ErrorMessage = "MajorId is required")] 
        public Guid MajorId { get; set; }
        public string? University { get; set; }
        public string? Industry { get; set; }
        [EnumValidation(typeof(UserRoleEnum))]
        public required string Role { get; set; }
    }

    public class RegisterResponseModel
    {
        public required string UserId { get; set; }
    }
}