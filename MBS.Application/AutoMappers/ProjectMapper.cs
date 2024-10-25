using AutoMapper;
using MBS.Application.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.Core.Entities;

namespace MBS.Application.AutoMappers
{
	public class ProjectMapper : Profile
	{
        public ProjectMapper()
        {
            CreateMap<Project, ProjectResponseDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        }
    }
}
