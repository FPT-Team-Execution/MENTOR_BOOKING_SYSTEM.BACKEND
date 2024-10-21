using MBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.Groups
{
    public class GroupModel
    {
        public GroupResponseDTO groupResponseDTO { get; set; }
    }

    public class GroupResponseDTO
    {
        public Guid ProjectId { get; set; }
        public string StudentId { get; set; }
        
        public Guid PositionId { get; set; }
    }

}
