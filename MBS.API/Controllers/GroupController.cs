using MBS.Application.Models.Groups;
using MBS.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MBS.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GroupController : ControllerBase
	{
		private readonly IGroupService _groupService;

		public GroupController(IGroupService groupService)
		{
			_groupService = groupService;
		}

		[HttpGet]
		[EndpointSummary("Get all available groups")]
		public async Task<IActionResult> GetAllGroups()
		{
			var response = await _groupService.GetAllGroup();
			return StatusCode((int)response.StatusCode, response);
		}

		[HttpGet("{id}")]
		[EndpointSummary("Get specific group by id")]
		public async Task<IActionResult> GetGroup(Guid id)
		{
			var response = await _groupService.GetGroup(new GetGroupRequestModel { Id = id });
			return StatusCode((int)response.StatusCode, response);
		}

		[HttpPost]
		[EndpointSummary("Create a new group")]
		public async Task<IActionResult> CreateGroup([FromBody] CreateNewGroupRequestModel request)
		{
			var response = await _groupService.CreateNewGroupAsync(request);
			return StatusCode((int)response.StatusCode, response);
		}

		[HttpPut("{id}")]
		[EndpointSummary("Update specific group by id")]
		public async Task<IActionResult> UpdateGroup(Guid id, [FromBody] UpdateGroupRequestModel request)
		{
			request.groupId = id;
			var response = await _groupService.UpdateGroup(request);
			return StatusCode((int)response.StatusCode, response);
		}

		[HttpDelete("{id}")]
		[EndpointSummary("Delete specific group by id")]
		public async Task<IActionResult> RemoveGroup(Guid id)
		{
			var response = await _groupService.RemoveGroup(new RemoveGroupRequestModel { GroupId = id });
			return StatusCode((int)response.StatusCode, response);
		}
	}
}
