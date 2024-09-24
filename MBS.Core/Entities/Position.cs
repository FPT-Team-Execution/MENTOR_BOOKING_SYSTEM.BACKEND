using MBS.Core.Common;
using MBS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Core.Entities
{
    public class Position : BaseEntity
    {
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        [MaxLength(20)]
        public StatusEnum Status  { get; set; }
    }
}
