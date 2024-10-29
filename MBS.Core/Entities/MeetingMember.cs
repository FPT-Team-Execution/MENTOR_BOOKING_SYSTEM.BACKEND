using MBS.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Core.Entities
{
    public class MeetingMember: BaseEntity
    {
        [Required]
        public Guid MeetingId { get; set; }
        [ForeignKey(nameof(MeetingId))]
        public Meeting Meeting { get; set; }
        [MaxLength(450), Required]
        public string StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }

        public DateTime JoinTime { get; set; }
        public DateTime? LeaveTime { get; set; }
    }
}
