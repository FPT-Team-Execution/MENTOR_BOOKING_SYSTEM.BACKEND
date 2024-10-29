using AutoMapper;
using MBS.Application.Models.MeetingMember;
using MBS.Core.Entities;

namespace MBS.Application.AutoMappers
{
	public class MeetingMemberMapper : Profile
	{
        public MeetingMemberMapper()
        {
            CreateMap<MeetingMember, MeetingMemberResponseDto>();
        }
    }
}
