using MBS.Core.Entities;
using MBS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.PointTransaction
{
    public class PointTransactionModel
    {
        public List<PointTransactionDTO> pointTransactionDTOs { get; set; }
    }

    public class PointTransactionDTO
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public int Amount { get; set; }
        public int RemainBalance { get; set; }
        public PointCurrencyEnum Currency { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public TransactionStatusEnum Status { get; set; }
        public TransactionKindEnum Kind { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
