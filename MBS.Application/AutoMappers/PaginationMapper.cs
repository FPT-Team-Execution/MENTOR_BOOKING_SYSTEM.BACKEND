using AutoMapper;
using MBS.Core.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.AutoMappers
{
	public class PaginationMapper : Profile
	{
        public PaginationMapper()
        {
			CreateMap(typeof(Pagination<>), typeof(Pagination<>));
		}
    }
}
