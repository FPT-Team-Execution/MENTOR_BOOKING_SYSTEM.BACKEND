

using MBS.Application.Models.Project;
using MBS.Core.Common.Pagination;

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
        [ProducesResponseType(typeof(BaseModel<Pagination<ProjectResponseDto>>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseModel),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseModel),StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProjectsByStudentId(GetProjectsByStudentIdRequest request)
        {
            var result = await _projectService.GetProjectsByStudentId(request);
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
        // [HttpPut("{projectId}/status")]
        // public async Task<IActionResult> UpdateProjectStatus([FromRoute] Guid projectId, [FromBody] ProjectStatusEnum newStatus)
        // {
        //     if (projectId == Guid.Empty)
        //     {
        //         return BadRequest("Invalid project ID.");
        //     }
        //
        //     var result = await _projectService.UpdateProjectStatus(projectId, newStatus);
        //
        //     return StatusCode(result.StatusCode, result);
        // }

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
