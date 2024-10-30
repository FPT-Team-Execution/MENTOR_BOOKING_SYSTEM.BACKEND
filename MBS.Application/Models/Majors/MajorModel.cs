using MBS.Core.Entities;
using MBS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.Majors
{
    public class MajorModel
    {
        public MajorResponseDTO MajorResponse { get; set; }
    }

    public class MajorResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? ParentName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

}
