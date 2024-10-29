using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MBS.Application.Models.Project
{
	public class GetAllProjectResponseModel
	{
        public IEnumerable<ProjectResponseDto> Projects { get; set; }
    }
	
	public class GetProjectsByStudentIdRequest
	{
		[FromRoute(Name = "studentId")]
		public string StudentId { get; set; }
		public string? ProjectStatus { get; set; }
		public int Page { get; set; } = 1;
		public int Size { get; set; } = 10;
		public string SortOrder { get; set; } = "asc";
	}

}
