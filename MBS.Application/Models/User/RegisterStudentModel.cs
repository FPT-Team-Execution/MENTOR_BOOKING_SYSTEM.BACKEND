using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.User
{
    public class RegisterStudentRequestModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Required] public required string Password { get; set; }

        [MaxLength(100)] [Required] public required string FullName { get; set; }

        public string? AvatarUrl { get; set; }

        [MaxLength(10)] [Required] public required string Gender { get; set; }

        public required Guid MajorId { get; set; }

        public string? University { get; set; }
    }

    public class RegisterStudentResponseModel
    {
        public required string UserId { get; set; }
    }
}