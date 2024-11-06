using MBS.Application.Models.Skill;
using MBS.Application.ValidationAttributes;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;


namespace MBS.API.Controllers;
[ApiController]
[Route("api/skills")]
[Authorize]
public class SkillController : ControllerBase
{
   private readonly ISkillService _skillService;
 
    public SkillController(ISkillService skillService)
    {
        _skillService = skillService;
    }
    [HttpGet]
    [ProducesResponseType(typeof(BaseModel<Pagination<SkillSummaryResponseDTO>>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSkills(int page, int size)
    {
        var result = await _skillService.Getskills(page, size);
        return StatusCode(result.StatusCode, result);
        
    }
    
    [HttpGet("{skillId}")]
    [ProducesResponseType(typeof(BaseModel<Pagination<SkillResponseModel>>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSkillById([FromRoute] Guid skillId)
    {
        var result = await _skillService.GetSkillById(skillId);
        return StatusCode(result.StatusCode, result);
        
    }
    [HttpGet("mentor/{mentorId}")]
    [ProducesResponseType(typeof(BaseModel<Pagination<Skill>>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSkillsByMentorId([FromRoute] string mentorId, int page, int size)
    {
        var result = await _skillService.GetSkillsByMentorId(mentorId, page,  size);
        return StatusCode(result.StatusCode, result);
        
    }
    [HttpPost("")]
    [CustomAuthorize(UserRoleEnum.Admin,UserRoleEnum.Mentor)]
    [ProducesResponseType(typeof(BaseModel<SkillResponseModel,CreateSkillRequestModel>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateSkill([FromBody]CreateSkillRequestModel requestModel)
    {
        var result = await _skillService.CreateSkill(requestModel);
        return StatusCode(result.StatusCode, result);
   
    }

    [HttpPut("{skillId}")]
    [CustomAuthorize(UserRoleEnum.Admin, UserRoleEnum.Mentor)]
    [ProducesResponseType(typeof(BaseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateSkill(Guid skillId, UpdateSkillRequestModel request)
    {
        var result = await _skillService.UpdateSkill(skillId, request);
        return StatusCode(result.StatusCode, result);

    }


    [HttpDelete("{skillId}")]
    [CustomAuthorize(UserRoleEnum.Admin,UserRoleEnum.Mentor)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseModel),StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteSkill([FromRoute] Guid skillId)
    {
        var result = await _skillService.DeleteSkill(skillId);
        return StatusCode(result.StatusCode, result);
        
    }
}