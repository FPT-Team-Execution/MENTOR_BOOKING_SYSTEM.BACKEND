using AutoMapper;
using MBS.Application.Models.Meeting;
using MBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.AutoMappers
{
	public class MeetingMapper : Profile
	{
        public MeetingMapper()
        {
            CreateMap<Meeting, MeetingResponseDto>();
        }
    }
}
