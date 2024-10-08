using MBS.Application.Models.Student;
using MBS.Core.Common.Pagination;

namespace MBS.API.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        // [Authorize(Roles = nameof(UserRoleEnum.Admin))]
        [ProducesResponseType(typeof(BaseModel<Pagination<StudentResponseDto>>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStudents(int page, int size)
        {
            var response = await _studentService.GetStudents(page, size);
            return StatusCode(response.StatusCode, response);
        }
        
        [HttpGet]
        [Route("profile")]
        [Authorize(Roles = nameof(UserRoleEnum.Student))]
        public async Task<ActionResult<BaseModel<GetStudentOwnProfileResponseModel>>>
            GetOwnProfile()
        {
            var response = await _studentService.GetOwnProfile(User);
            return StatusCode(response.StatusCode, response);
        }
    }
}