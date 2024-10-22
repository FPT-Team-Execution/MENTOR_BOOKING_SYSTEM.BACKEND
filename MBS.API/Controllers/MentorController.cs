using MBS.Core.Common.Pagination;

namespace MBS.API.Controllers
{
    [Route("api/mentors")]
    [ApiController]
    public class MentorController : ControllerBase
    {
        private readonly IMentorService _mentorService;

        public MentorController(IMentorService mentorService)
        {
            _mentorService = mentorService;
        }

        [HttpGet]
        [Route("profile")]
        [Authorize(Roles = nameof(UserRoleEnum.Mentor))]
        public async Task<ActionResult<BaseModel<GetMentorOwnProfileResponseModel>>>
            GetOwnProfile()
        {
            var response = await _mentorService.GetOwnProfile(User);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        [Route("degree")]
        [Authorize(Roles = nameof(UserRoleEnum.Mentor))]
        public async Task<ActionResult<BaseModel<UploadOwnDegreeResponseModel, UploadOwnDegreeRequestModel>>>
            UploadOwnDegree(UploadOwnDegreeRequestModel request)
        {
            var response = await _mentorService.UploadOwnDegree(request, User);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [Route("degree")]
        [Authorize(Roles = nameof(UserRoleEnum.Mentor))]
        public async Task<ActionResult<BaseModel<GetOwnDegreesResponseModel>>>
            GetOwnDegrees()
        {
            var response = await _mentorService.GetOwnDegrees(User);
            return StatusCode(response.StatusCode, response);
        }


        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = nameof(UserRoleEnum.Admin))]
        public async Task<IActionResult> GetMentor([FromRoute] string id)
        {
            var response = await _mentorService.GetMentor(new GetMentorRequestModel()
            {
                Id = id
            });
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRoleEnum.Admin))]
        [ProducesResponseType(typeof(BaseModel<Pagination<GetMentorResponseModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMentors(int page, int size)
        {
            var response = await _mentorService.GetMentors(page, size);
            return StatusCode(response.StatusCode, response);
        }
    }
}