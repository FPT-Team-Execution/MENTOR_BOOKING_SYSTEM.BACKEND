using AutoMapper;
using MBS.Application.Models.CalendarEvent;
using MBS.Core.Entities;

namespace MBS.Application.AutoMappers;

public class EventMapper : Profile
{
    public EventMapper()
    {
        CreateMap<CalendarEvent, BusyEventModel>()
            .ForMember(des => des.Start,opt => opt.MapFrom(src => src.Start))
            .ForMember(des => des.End,opt => opt.MapFrom(src => src.End));
    }
}