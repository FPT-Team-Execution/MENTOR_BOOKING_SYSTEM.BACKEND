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
    public class MentorMajor : BaseEntity
    {
        public Guid MajorId { get; set; }
        [ForeignKey(nameof(MajorId))]
        public Major Major { get; set; }

        [MaxLength(450)]
        public string MentorId { get; set; }
        [ForeignKey(nameof(MentorId))]
        public Mentor Mentor { get; set; }
    }
}
