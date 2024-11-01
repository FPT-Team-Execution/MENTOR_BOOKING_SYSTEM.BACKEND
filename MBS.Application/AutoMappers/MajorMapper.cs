﻿using AutoMapper;
using MBS.Application.Models.Majors;
using MBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.AutoMappers
{
    public class MajorMapper : Profile
	{
		public MajorMapper() 
		{
			CreateMap<Major, MajorResponseDTO>();
		}
	}
}
