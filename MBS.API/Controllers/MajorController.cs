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
        [Route("/GetAll")]
        public async Task<IActionResult> GetAllMajors()
        {
            var response = await _majorService.GetAllMajor();
            return Ok(response);
        }
        //Tested
        [HttpGet]
        [Route("/GetMajor")]
        public async Task<IActionResult> GetMajor(Guid resquestId)
        {
            var response = await _majorService.GetMajor(new GetMajorRequestModel { id = resquestId });
            return Ok(response);
        }
        //Tested
        [HttpPost]
        [Route("/CreateMajor")]
        public async Task<IActionResult> CreateMajor([FromBody] CreateMajorRequestModel request)
        {
            var response = await _majorService.CreateNewMajorAsync(request);
            return StatusCode((int)response.StatusCode, response);
        }
        //Testing
        [HttpPut]
        [Route("/UpdateMajor/{id}")]
        public async Task<IActionResult> UpdateMajor(Guid id, [FromBody] UpdateMajorRequestModel request)
        {
            request.id = id;
            var response = await _majorService.UpdateMajor(request);
            return StatusCode((int)response.StatusCode, response);
        }
        //Tested
        [HttpDelete]
        [Route("/DeleteMajor")]
        public async Task<IActionResult> RemoveMajor(Guid resquestId)
        {
            var response = await _majorService.RemoveMajor(new RemoveMajorRequestModel { id = resquestId});
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
