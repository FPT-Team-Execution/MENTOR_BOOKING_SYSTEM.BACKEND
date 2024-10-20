using MBS.Application.Models.Groups;
using MBS.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MBS.API.Controllers
{


	[Route("api/groups")]
	[ApiController]
	public class GroupController : ControllerBase
	{
		private readonly IGroupService _groupService;

		public GroupController(IGroupService groupService)
		{
			_groupService = groupService;
		}
		//Tested
		[HttpGet]
		[EndpointSummary("Get all available groups")]
		public async Task<IActionResult> GetAllGroups(int page, int size)
		{
			var response = await _groupService.GetGroups(page, size);
			return StatusCode((int)response.StatusCode, response);
		}
		//Tested
		[HttpGet("{id}")]
		[EndpointSummary("Get specific group by id")]
		public async Task<IActionResult> GetGroup(Guid id)
		{
			var response = await _groupService.GetGroupId(id);
			return StatusCode((int)response.StatusCode, response);
		}
		//Tested
		[HttpPost]
		[EndpointSummary("Create a new group")]
		public async Task<IActionResult> CreateGroup([FromBody] CreateNewGroupRequestModel request)
		{
			var response = await _groupService.CreateNewGroupAsync(request);
			return StatusCode((int)response.StatusCode, response);
		}
		//Tested
		[HttpPut("{id}")]
		[EndpointSummary("Update specific group by id")]
		public async Task<IActionResult> UpdateGroup(Guid id, [FromBody] UpdateGroupRequestModel request)
		{
			var response = await _groupService.UpdateGroup(id, request);
			return StatusCode((int)response.StatusCode, response);
		}
		//Tested
		[HttpDelete("{id}")]
		[EndpointSummary("Delete specific group by id")]
		public async Task<IActionResult> RemoveGroup(Guid id)
		{
			var response = await _groupService.RemoveGroup(id);
			return StatusCode((int)response.StatusCode, response);
		}

		[HttpGet("students/{id}")]
		public async Task<IActionResult> GetStudentsInGroup(Guid id)
		{
			var response = await _groupService.GetStudentsInGroupByProjectId(id);
			return StatusCode((int)response.StatusCode, response);


        }
    }
}
