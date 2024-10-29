using MBS.Core.Common;
using MBS.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MBS.Core.Entities
{
    public class PointTransaction : BaseEntity
    {
        [MaxLength(450)]
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }
        public int Amount { get; set; }
        public int RemainBalance { get; set; }
        [MaxLength(10)]
        public PointCurrencyEnum Currency { get; set; }
		//TransactionType is action debit or credit
		[MaxLength(50)]
        public TransactionTypeEnum TransactionType { get; set; }
		//Status is transaction status: success or fail
		[MaxLength(20)]
		public TransactionStatusEnum Status { get; set; }
		//Kind is property to check transaction for project or personal
		[MaxLength(20)]
		public TransactionKindEnum Kind { get; set; }
		public DateTime CreatedOn { get; set; }
   
    }
}
