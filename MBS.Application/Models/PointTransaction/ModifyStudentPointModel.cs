using MBS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.PointTransaction
{
    public class ModifyStudentPointRequestModel
    {
        public required string StudentId { get; set; }
        public required int Amout { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
    }

    public class ModifyStudentPointResponseModel
    {
        public required string StudentId { get; set; }
        public required int TotalAmount { get; set; }
    }
}
