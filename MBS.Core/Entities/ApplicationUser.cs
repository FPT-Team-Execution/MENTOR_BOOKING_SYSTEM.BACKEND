using MBS.Core.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Core.Entities
{
    public class ApplicationUser : IdentityUser, IAuditedEntity
    {
        [MaxLength(100)]
        public string FullName { get; set; } = default;
        public string? AvatarUrl { get; set; } = default;
        [MaxLength(10)]
        public string Gender { get; set; } = default;
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
