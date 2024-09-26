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