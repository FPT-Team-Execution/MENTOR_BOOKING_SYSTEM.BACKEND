namespace MBS.API.Controllers
{
    [Route("api/[controller]")]
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
            await Task.Delay(1000);
            return Ok();
        }
    }
}