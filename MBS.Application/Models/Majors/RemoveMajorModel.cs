using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.Majors
{
    public class RemoveMajorRequestModel
    {
        public required Guid id{ get; set; }
        

    }
    public class RemoveMajorResponseModel
    {
        public string success { get; set; }
    }
}
