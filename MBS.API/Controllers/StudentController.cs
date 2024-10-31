using MBS.Application.Models.Student;
using MBS.Application.ValidationAttributes;
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
		[Authorize(Roles = nameof(UserRoleEnum.Admin))]
		[ProducesResponseType(typeof(BaseModel<Pagination<StudentResponseDto>>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(BaseModel), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetStudents(int page, int size, string? sortOrder)
		{
			var response = await _studentService.GetStudents(page, size, sortOrder);
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet]
		[Route("profile")]
		[Authorize(Roles = nameof(UserRoleEnum.Student))]
		public async Task<IActionResult>
			GetOwnProfile()
		{
			var response = await _studentService.GetOwnProfile(User);
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet]
		[Route("{id}")]
		[Authorize(Roles = nameof(UserRoleEnum.Admin))]
		public async Task<IActionResult> GetStudent([FromRoute] string id)
		{
			var response = await _studentService.GetStudent(new GetStudentRequestModel()
			{
				Id = id
			});
			return StatusCode(response.StatusCode, response);
		}

		[HttpPut]
		[Route("profile")]
		[Authorize]
		public async Task<IActionResult> UpdateStudent(UpdateStudentRequestModel request)
		{
			var response = await _studentService.UpdateStudentProfile(request);
			return StatusCode(response.StatusCode, response);
		}

		[HttpPost]
		[Route("")]
		[Authorize(Roles = nameof(UserRoleEnum.Admin))]
		public async Task<IActionResult> CreateStudent(CreateStudentRequestModel request)
		{
			var response = await _studentService.CreateStudent(request);
			return StatusCode(response.StatusCode, response);
		}
	}
}