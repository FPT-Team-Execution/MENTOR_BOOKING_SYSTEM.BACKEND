using MBS.Core.Common;
using MBS.Core.Enums;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MBS.Core.Entities
{
    public class PointTransaction : BaseEntity, IAuditedEntity
    {
        [MaxLength(450)]
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
        [MaxLength(50)]
        public TransactionTypeEnum TransactionType { get; set; }
        public int Amount { get; set; }
        [MaxLength(10)]
        public PointCurrencyEnum Currency { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
