using AutoMapper;
using MBS.Application.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.AutoMappers
{
	public class ProjectMapper : Profile
	{
        public ProjectMapper()
        {
            CreateMap<Profile, ProjectResponseDto>();
        }
    }
}
