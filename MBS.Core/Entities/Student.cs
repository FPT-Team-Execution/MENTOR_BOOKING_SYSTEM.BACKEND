
using MBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Core.Entities
{
    public class Student 
    {
        [Key]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public string? University { get; set; } = default;
        public int WalletPoint { get; set; } = default;

        public Guid MajorId { get; set; }
        [ForeignKey(nameof(MajorId))]
        public Major Major { get; set; }
    }
}
