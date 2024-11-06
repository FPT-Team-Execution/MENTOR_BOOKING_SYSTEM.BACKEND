using AutoMapper;
using MBS.Application.Models.Progress;
using MBS.Core.Entities;

namespace MBS.Application.AutoMappers;

public class ProgressMapper : Profile
{
    public ProgressMapper() 
    {
        CreateMap<Progress, ProgressResponseDto>();
    }
}