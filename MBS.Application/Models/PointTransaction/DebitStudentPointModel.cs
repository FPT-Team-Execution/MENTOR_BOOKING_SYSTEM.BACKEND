using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.PointTransaction
{
    public class DebitStudentPointRequestModel
    {
        public required string StudentId { get; set; }
        public required int DebitAmout { get; set; }
    }

    public class DebitStudentPointResponseModel
    {
        public required string StudentId { get; set; }
        public required int TotalAmout { get; set; }
    }
}
