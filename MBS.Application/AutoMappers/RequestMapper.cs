using AutoMapper;
using MBS.Application.Models.Request;
using MBS.Core.Entities;

namespace MBS.Application.AutoMappers;

public class RequestMapper : Profile
{
    public RequestMapper()
    {
        CreateMap<Request, RequestResponseDto>();

    }
}