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
    public class Request : BaseEntity, IAuditedEntity
    {
        [MaxLength(100)]
        public string Title { get; set; }
        //public  string CalendarEventId { get; set; }
        //[ForeignKey(nameof(CalendarEventId))]
        //public CalendarEvent CalendarEvent { get; set; }
        public DateTime Start {  get; set; }
        public DateTime End { get; set; }
        public  string MentorId { get; set; }
        [ForeignKey(nameof(MentorId))]
        public Mentor Mentor { get; set; }
        public Guid? ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }
        [MaxLength(450)]
        public string CreaterId { get; set; }
        [ForeignKey(nameof(CreaterId))]
        public Student Creater { get; set; }
        [MaxLength(20)]
        public RequestStatusEnum Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
