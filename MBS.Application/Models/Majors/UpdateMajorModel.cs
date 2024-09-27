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
        [Required]
        public Guid id {  get; set; }
        public string Name { get; set; }

        public Guid? ParentId { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public StatusEnum Status { get; set; }
    }

    public class UpdateMajorResponseModel
    {
        public Major UpdatedMajor { get; set; }
    }
}
