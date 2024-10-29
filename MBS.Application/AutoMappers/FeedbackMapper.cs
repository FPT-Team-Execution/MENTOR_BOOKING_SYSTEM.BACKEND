using AutoMapper;
using MBS.Application.Models.Feedback;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.AutoMappers
{
	public class FeedbackMapper : Profile
	{
        public FeedbackMapper()
        {
            CreateMap<Feedback, FeedbackResponseDto>();
		}
    }
}
