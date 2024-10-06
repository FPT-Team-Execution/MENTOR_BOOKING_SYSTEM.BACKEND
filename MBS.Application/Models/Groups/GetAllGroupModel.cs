using MBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.Groups
{
    public class GetAllGroupRequestModel
    {

    }
    public class GetAllGroupResponseModel
    {
        public List<Group> groups {  get; set; }
    }
}
