using MBS.Application.Models.Majors;
using MBS.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MBS.Application.ValidationAttributes;

namespace MBS.API.Controllers
{
	[Route("api/majors")]
	[ApiController]
	public class MajorController : ControllerBase
	{
		private readonly IMajorService _majorService;

		public MajorController(IMajorService majorService)
		{
			_majorService = majorService;
		}
		//Tested
		[HttpGet]
		[EndpointSummary("Get all available major")]
		public async Task<IActionResult> GetAllMajors(int page, int size)
		{
			var response = await _majorService.GetMajors(page, size);
			return Ok(response);
		}
		//Tested
		[HttpGet("{id}")]
		[Authorize]
		[EndpointSummary("Get specific major by id")]
		public async Task<IActionResult> GetMajor(Guid id)
		{
			var response = await _majorService.GetMajorId(id);
			return Ok(response);
		}
		//Tested
		[HttpPost]
		[EndpointSummary("Create new major")]
		[CustomAuthorize(UserRoleEnum.Admin)]
		public async Task<IActionResult> CreateMajor([FromBody] CreateMajorRequestModel request)
		{
			var response = await _majorService.CreateNewMajorAsync(request);
			return StatusCode((int)response.StatusCode, response);
		}
		//Tested
		[HttpPut("{id}")]
		[EndpointSummary("Update specific major by id")]
		[CustomAuthorize(UserRoleEnum.Admin)]
		public async Task<IActionResult> UpdateMajor(Guid id, [FromBody] UpdateMajorRequestModel request)
		{
			var response = await _majorService.UpdateMajor(id, request);
			return StatusCode((int)response.StatusCode, response);
		}
		//Tested
		[HttpDelete("{id}")]
		[CustomAuthorize(UserRoleEnum.Admin)]
		[EndpointSummary("Delete specific major by id")]
		public async Task<IActionResult> RemoveMajor(Guid id)
		{
			var response = await _majorService.RemoveMajor(id);
			return StatusCode((int)response.StatusCode, response);
		}

		[HttpGet]
		[Authorize]
		[Route("mentor/{id}")]
		[EndpointSummary("Get major by mentorId specific major by id")]
		public async Task<IActionResult> GetMajorByMentorId(GetMentorMajorsRequest request)
		{
			var response = await _majorService.GetMentorMajors(request);
			return StatusCode(response.StatusCode, response);
		}
	}
}