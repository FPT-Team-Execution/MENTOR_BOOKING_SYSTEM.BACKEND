
using MBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.Groups
{
    public class GetGroupRequestModel
    {
        public required Guid Id { get; set; }
    }

    public class GetGroupResponseModel
    {
      public Group groupResponse {  get; set; } 
    }
}
