using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.Groups
{
    public class RemoveGroupRequestModel
    {
        public Guid GroupId { get; set; }
    }
    public class RemoveGroupResponseModel
    {
        public bool? Success { get; set; }
    }
}
