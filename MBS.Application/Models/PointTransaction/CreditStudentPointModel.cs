using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.PointTransaction
{
    public class CreditStudentPointRequestModel
    {
        public required string StudentId { get; set; }
        public required int CreditAmout { get; set; }
    }

    public class CreditStudentPointResponseModel
    {
        public required string StudentId { get; set; }
        public required int TotalAmout { get; set; }
    }
}
