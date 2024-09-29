using MBS.Application.Models.Majors;
using MBS.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MBS.API.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> GetAllMajors()
        {
            var response = await _majorService.GetAllMajor();
            return Ok(response);
        }
        //Tested
        [HttpGet ("{id}")]
        public async Task<IActionResult> GetMajor(Guid id)
        {
            var response = await _majorService.GetMajor(new GetMajorRequestModel { id = id});
            return Ok(response);
        }
        //Tested
        [HttpPost]
        public async Task<IActionResult> CreateMajor([FromBody] CreateMajorRequestModel request)
        {
            var response = await _majorService.CreateNewMajorAsync(request);
            return StatusCode((int)response.StatusCode, response);
        }
        //Testing
        [HttpPut ("{id}")]
        public async Task<IActionResult> UpdateMajor(Guid id, [FromBody] UpdateMajorRequestModel request)
        {
            request.id = id;
            var response = await _majorService.UpdateMajor(request);
            return StatusCode((int)response.StatusCode, response);
        }
        //Tested
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveMajor(Guid id)
        {
            var response = await _majorService.RemoveMajor(new RemoveMajorRequestModel { id = id});
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
