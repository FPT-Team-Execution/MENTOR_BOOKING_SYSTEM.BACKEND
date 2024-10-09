using MBS.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.Positions
{
    public class PositionModel
    {
        public PositionResponseDTO positionResponse { get; set; }
    }

    public class PositionResponseDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public StatusEnum Status { get; set; }
    }
}
