using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.Project
{
	public class GetAllProjectResponseModel
	{
        public IEnumerable<ProjectResponseDto> Projects { get; set; }
    }
}
