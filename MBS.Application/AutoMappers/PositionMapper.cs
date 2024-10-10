using AutoMapper;
using MBS.Application.Models.Positions;
using MBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.AutoMappers
{
    class PositionMapper : Profile
    {
        public PositionMapper() 
        {
            CreateMap<Position, PositionResponseDTO>();
        }
    }
}
