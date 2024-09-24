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
    public class Skill : BaseEntity
    {
        [MaxLength(100), Required]
        public string Name { get; set; } = default!;
        [MaxLength(450), Required]
        public string MentorId { get; set; }
        [ForeignKey(nameof(MentorId))]
        public Mentor Mentor { get; set; }
    }
}
