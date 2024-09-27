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
    public class CalendarEvent 
    {
        [Key]
        public string Id { get; set; }
        public string HtmlLink { get; set; }
        public string Summary { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ICalUID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public Guid? MeetingId { get; set; }
        [ForeignKey(nameof(MeetingId))]
        public Meeting? Meeting { get; set; }
        [MaxLength(450)]
        public string MentorId { get; set; }
        [ForeignKey(nameof(MentorId))]
        public Mentor Mentor { get; set; }
        
        [MaxLength(20)]
        public EventStatus Status { get; set; }

    }
}
