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
    public class CalendarEvent : BaseEntity
    {
        public Guid MeetingId { get; set; }
        [ForeignKey(nameof(MeetingId))]
        public Meeting Meeting { get; set; }

        [MaxLength(450)]
        public string MentorId { get; set; }
        [ForeignKey(nameof(MentorId))]
        public Mentor Mentor { get; set; }
        [MaxLength(20)]
        public string Label { get; set; }
        [MaxLength(20)]
        public string LabelColor { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public DateTime StartTime  { get; set; }
        public DateTime? EndTime { get; set; }
        [MaxLength(20)]
        public StatusEnum Status { get; set; }

    }
}
