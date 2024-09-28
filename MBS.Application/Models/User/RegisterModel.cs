using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.Core.Enums;

namespace MBS.Application.Models.User
{
    public class RegisterRequestModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Required] public required string Password { get; set; }

        [MaxLength(100)] [Required] public required string FullName { get; set; }

        [MaxLength(10)] [Required] public required string Gender { get; set; }

        public Guid MajorId { get; set; }

        public string? University { get; set; }

        public string? Industry { get; set; }

        public required string Role { get; set; }
    }

    public class RegisterResponseModel
    {
        public required string UserId { get; set; }
    }
}