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
    public class UpdateMajorRequestModel
    {
        public required Guid id {  get; set; }
        public string Name { get; set; }
        public Guid? ParentId { get; set; }
    }

    public class UpdateMajorResponseModel
    {
        public Major UpdatedMajor { get; set; }
    }
}
