using MBS.Application.Models.PointTransaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MBS.API.Controllers
{
    [Route("api/point-transactions")]
    [ApiController]
    public class PointTransactionController : ControllerBase
    {
        private readonly IPointTransactionSerivce _pointTransactionSerivce;

        public PointTransactionController(IPointTransactionSerivce pointTransactionSerivce)
        {
            _pointTransactionSerivce = pointTransactionSerivce;
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRoleEnum.Admin))]
        public async Task<IActionResult> ModifyStudentPoint(ModifyStudentPointRequestModel request)
        {
            var response = await _pointTransactionSerivce.ModifyStudentPoint(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRoleEnum.Admin))]
        public async Task<IActionResult> GetAllPointTransaction([FromQuery] int page, [FromQuery] int size)
        {
            var response = await _pointTransactionSerivce.GetAllPointTransaction(page, size);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{studentId}")]
        [Authorize(Roles = nameof(UserRoleEnum.Admin))]
        public async Task<IActionResult> GetAllPointTransaction([FromRoute] string studentId,[FromQuery] int page, [FromQuery] int size)
        {
            var response = await _pointTransactionSerivce.GetPointTransactionByStudentId(studentId, page, size);
            return StatusCode(response.StatusCode, response);
        }
    }
}
