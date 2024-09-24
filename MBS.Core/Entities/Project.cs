using MBS.Core.Common;
using MBS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Core.Entities
{
    public class Project : BaseEntity, IAuditedEntity
    {
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        [MaxLength(50)]
        public string Semester { get; set; }
        public string CreatedBy { get; set; }
        [MaxLength(450)]
        public string MentorId { get; set; }
        [ForeignKey(nameof(MentorId))]
        public Mentor Mentor { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        [MaxLength(20)]
        public ProjectStatusEnum Status { get; set; }
    }
}
