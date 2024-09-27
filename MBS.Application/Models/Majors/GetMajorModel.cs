using MBS.Core.Entities;
using MBS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.Majors
{
    public class GetMajorRequestModel
    {
        public required Guid id { get; set; }

    }
    public class GetMajorResponseModel
    {
        public Major major { get; set; }
    }
}
