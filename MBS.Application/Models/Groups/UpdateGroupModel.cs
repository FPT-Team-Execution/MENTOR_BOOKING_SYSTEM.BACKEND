using MBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.Groups
{
    public class UpdateGroupRequestModel
    {
        public required string studentId { get; set; }
        [Required]
        public Guid PositionId { get; set; }
    }
    public class UpdateGroupResponseModel
    {
        public Guid id { get; set; }
    }
}
