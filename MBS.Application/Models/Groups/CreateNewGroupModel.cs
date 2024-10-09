using MBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.Groups
{
    public class CreateNewGroupRequestModel
    {
        public Guid projectId { get; set; }
        public string StudentId { get; set; }
        public Guid PositionId { get; set; }

    }

    public class CreateNewGroupResponseModel
    {
        public Guid id { get; set; }
    }
}
