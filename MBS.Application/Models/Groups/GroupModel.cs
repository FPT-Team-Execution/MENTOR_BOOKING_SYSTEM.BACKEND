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
        public string Name { get; set; }
        public string StudentName { get; set; }

        public string PositionName { get; set; }
    }

}
