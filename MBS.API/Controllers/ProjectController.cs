

using MBS.Application.Models.Project;

namespace MBS.API.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // POST: api/projects
        [HttpPost("")]
        public async Task<ActionResult<BaseModel<CreateProjectResponseModel, CreateProjectRequestModel>>> CreateProject([FromBody] CreateProjectRequestModel request)
        {
            var result = await _projectService.CreateProject(request);
            return StatusCode(result.StatusCode, result);
        }

        // GET: api/projects/student/{studentId}?projectStatus=Active
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetProjectsByStudentId([FromRoute] string studentId, [FromQuery] ProjectStatusEnum? projectStatus)
        {
            if (string.IsNullOrWhiteSpace(studentId))
            {
                return BadRequest("Invalid student ID.");
            }

            var result = await _projectService.GetProjectsByStudentId(studentId, projectStatus);
            return StatusCode(result.StatusCode, result);
        }

        // PUT: api/projects/{projectId}
        [HttpPut("{projectId}")]
        public async Task<IActionResult> UpdateProject([FromRoute] Guid projectId, [FromBody] UpdateProjectRequestModel request)
        {
            if (projectId == Guid.Empty)
            {
                return BadRequest("Invalid project Id.");
            }

            var result = await _projectService.UpdateProject(projectId, request);
            return StatusCode(result.StatusCode, result);
        }

        // PUT: api/projects/{projectId}/status
        [HttpPut("{projectId}/status")]
        public async Task<IActionResult> UpdateProjectStatus([FromRoute] Guid projectId, [FromBody] ProjectStatusEnum newStatus)
        {
            if (projectId == Guid.Empty)
            {
                return BadRequest("Invalid project ID.");
            }

            var result = await _projectService.UpdateProjectStatus(projectId, newStatus);

            return StatusCode(result.StatusCode, result);
        }

        // GET: api/projects/{projectId}
        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProjectById([FromRoute] Guid projectId)
        {
            if (projectId == Guid.Empty)
            {
                return BadRequest("Invalid project ID.");
            }

            var result = await _projectService.GetProjectById(projectId);
            return StatusCode(result.StatusCode, result);
        }

        // PUT: api/projects/{projectId}/mentor/{mentorId}
        [HttpPut("{projectId}/mentor/{mentorId}")]
        public async Task<IActionResult> AssignMentor([FromRoute] Guid projectId, [FromRoute] string mentorId)
        {
            if (projectId == Guid.Empty || string.IsNullOrWhiteSpace(mentorId))
            {
                return BadRequest("Invalid project or mentor ID.");
            }

            var result = await _projectService.AssignMentor(projectId, mentorId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
