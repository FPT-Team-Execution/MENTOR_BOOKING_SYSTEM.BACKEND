using MBS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.Core.Entities;
namespace MBS.Application.Models.Majors
{
    public class CreateMajorRequestModel
    {
        public required string Name { get; set; }
        public Guid? ParentId { get; set; }
    }

    public class CreateMajorResponseModel
    {
        public Guid MajorId { get; set; }
    }
}
