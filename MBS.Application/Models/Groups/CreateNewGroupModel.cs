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
        public required Guid ProjectId { get; set; }
        public required string StudentId { get; set; }
        public required Guid PositionId { get; set; }

    }

    public class CreateNewGroupResponseModel
    {
        public Guid Id { get; set; }
    }
}
