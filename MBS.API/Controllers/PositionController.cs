using MBS.Application.Models.Positions;
using MBS.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MBS.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PositionController : ControllerBase
	{
		private readonly IPositionService _positionService;

		public PositionController(IPositionService positionService)
		{
			_positionService = positionService;
		}

		[HttpGet]
		[EndpointSummary("Get all available positions")]
		public async Task<IActionResult> GetAllPositions()
		{
			var response = await _positionService.GetAllPosition();
			return StatusCode((int)response.StatusCode, response);
		}

		[HttpGet("{id}")]
		[EndpointSummary("Get specific position by id")]
		public async Task<IActionResult> GetPosition(Guid id)
		{
			var response = await _positionService.GetPosition(new GetPositionRequestModel { Id = id });
			return StatusCode((int)response.StatusCode, response);
		}

		[HttpPost]
		[EndpointSummary("Create a new position")]
		public async Task<IActionResult> CreatePosition([FromBody] CreatePositionRequestModel request)
		{
			var response = await _positionService.CreateNewPositionAsync(request);
			return StatusCode((int)response.StatusCode, response);
		}

		[HttpPut("{id}")]
		[EndpointSummary("Update specific position by id")]
		public async Task<IActionResult> UpdatePosition(Guid id, [FromBody] UpdatePositionRequestModel request)
		{
			var response = await _positionService.UpdatePosition(id, request);
			return StatusCode((int)response.StatusCode, response);
		}


		[HttpDelete("{id}")]
		[EndpointSummary("Delete specific position by id")]
		public async Task<IActionResult> RemovePosition(Guid id)
		{
			var response = await _positionService.RemovePosition(new RemovePositionRequestModel { Id = id });
			return StatusCode((int)response.StatusCode, response);
		}
	}
}
