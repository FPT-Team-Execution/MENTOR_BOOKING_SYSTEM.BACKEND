using MBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.Majors
{
    public class GetAllMajorReuqestModel
    {
    }

    public class GetAllMajorResponseModel
    {
        public List<Major> majors { get; set; }
    }
}
