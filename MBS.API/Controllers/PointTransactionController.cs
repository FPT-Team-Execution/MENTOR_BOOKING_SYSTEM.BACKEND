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
        public async Task<IActionResult> CreditStudentPoint(CreditStudentPointRequestModel request)
        {
            var response = await _pointTransactionSerivce.CreditStudentPoint(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRoleEnum.Admin))]
        public async Task<IActionResult> DebitStudentPoint(DebitStudentPointRequestModel request)
        {
            var response = await _pointTransactionSerivce.DebitStudentPoint(request);
            return StatusCode(response.StatusCode, response);
        }

    }
}
