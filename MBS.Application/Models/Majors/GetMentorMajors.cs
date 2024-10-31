using MBS.Core.Common.Pagination;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.Majors
{
	public class GetMentorMajorsRequest
	{
		[FromRoute(Name = "id")]
		public required string MentorId { get; set; }
		[FromQuery]
		public int Page { get; set; } = 1;
		[FromQuery]
		public int Size { get; set; } = 100;
	}

	public class GetMentorMajorsResponse
	{
		public required Pagination<MajorResponseDTO> Majors { get; set; }
	}
}
