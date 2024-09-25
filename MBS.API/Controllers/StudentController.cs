using MBS.Application.Models.General;
using MBS.Application.Models.User;
using MBS.Application.Services.Interfaces;
using MBS.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MBS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        [Route("student/profile")]
        [Authorize(Roles = nameof(UserRoleEnum.Student))]
        public async Task<ActionResult<BaseModel<GetRefreshTokenResponseModel>>>
            GetStudentOwnProfile()
        {
            var response = await _studentService.GetStudentOwnProfile(User);
            return StatusCode(response.StatusCode, response);
        }
    }
}