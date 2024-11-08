using MBS.Application.Models.CalendarEvent;
using MBS.Application.Models.PointTransaction;
using MBS.Application.ValidationAttributes;
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
            var response = await _pointTransactionSerivce.GetAllPointTransactionPageListAsync(page, size);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{studentId}")]
        [CustomAuthorize(UserRoleEnum.Admin, UserRoleEnum.Student)]
        [ProducesResponseType(typeof(BaseModel<CreateCalendarResponseModel, CreateCalendarRequestModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPointTransactionByStudentId([FromRoute] string studentId, [FromQuery] int page, [FromQuery] int size)
        {
            var response = await _pointTransactionSerivce.GetPointTransactionByStudentId(studentId, page, size);
            return StatusCode(response.StatusCode, response);
        }
    }
}
