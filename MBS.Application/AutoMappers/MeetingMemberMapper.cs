using AutoMapper;
using MBS.Application.Models.MeetingMember;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.AutoMappers
{
	public class MeetingMemberMapper : Profile
	{
        public MeetingMemberMapper()
        {
            CreateMap<MeetingMapper, MeetingMemberResponseDto>();
        }
    }
}
