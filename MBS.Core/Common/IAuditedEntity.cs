using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Core.Common
{
    public interface IAuditedEntity
    {
        [MaxLength(450)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
        [MaxLength(450)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
