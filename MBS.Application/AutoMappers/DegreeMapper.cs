using AutoMapper;
using MBS.Application.Models.User;
using MBS.Core.Entities;

namespace MBS.Application.AutoMappers;

public class DegreeMapper : Profile
{
    public DegreeMapper()
    {
        CreateMap<Degree, GetOwnDegreeResponseModel>();
    }
}