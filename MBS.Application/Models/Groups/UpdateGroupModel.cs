using MBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.Groups
{
    public class UpdateGroupRequestModel
    {
        public string studentId { get; set; }
        public Guid PositionId { get; set; }
    }
    public class UpdateGroupResponseModel
    {
        public Guid id { get; set; }
    }
}
