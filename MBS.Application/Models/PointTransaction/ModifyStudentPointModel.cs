using MBS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.Application.ValidationAttributes;

namespace MBS.Application.Models.PointTransaction
{
    public class ModifyStudentPointRequestModel
    {
        public required string StudentId { get; set; }
        public required int Amount { get; set; }
        [EnumValidation(typeof(TransactionTypeEnum))]
        public string TransactionType { get; set; }
        public string Kind { get; set; }
    }

    public class ModifyStudentPointResponseModel
    {
        public required string StudentId { get; set; }
        public required int TotalAmount { get; set; }
    }
}
