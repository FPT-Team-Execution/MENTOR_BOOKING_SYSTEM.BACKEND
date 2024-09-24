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
    public class Major : BaseEntity, IAuditedEntity
    {
        [MaxLength(225), Required]
        public string Name { get; set; }
      
        public Guid ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public Major ParentMajor { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        [MaxLength(20), Required]
        public StatusEnum Status { get; set; }
    }
}
