using MBS.Application.Models.User;
using MBS.Core.Common.Pagination;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Models.Mentor
{
	public class GetMentorDegreesRequestModel
	{
		[FromRoute(Name = "id")]
		public required string MentorId { get; set; }
		[FromQuery]
		public required int Page { get; set; }
		[FromQuery]
		public required int Size { get; set; }
	}

	public class GetMentorDegreeResponseModel
	{
		public required Guid Id { get; set; }
		public string Name { get; set; } = default;
		public string? ImageUrl { get; set; } = default;
		public string? Institution { get; set; } = default;

	}

	public class GetMentorDegreesResponseModel
	{
		public required Pagination<GetMentorDegreeResponseModel> Degrees;

	}
}
