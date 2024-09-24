
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MBS.Core.Entities
{
    public class Mentor
    {
        [Key]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public string? Industry { get; set; } = default;
        public int ConsumePoint { get; set; } = default;
    }
}
