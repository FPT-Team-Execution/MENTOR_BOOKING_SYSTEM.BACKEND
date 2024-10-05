using MBS.Application.Models.General;
using MBS.Application.Models.Skill;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;

namespace MBS.Application.Services.Interfaces;

public interface ISkillService
{
    Task<BaseModel<SkillResponseModel, CreateSkillRequestModel>> CreateSkill(CreateSkillRequestModel request);
    Task<BaseModel<Pagination<Skill>>> GetSkillsByMentorId(string mentorId, int page, int size);

    Task<BaseModel<Pagination<Skill>>> Getskills(int page, int size);
    Task<BaseModel<SkillResponseModel>> GetSkillById(Guid skillId);
    Task<BaseModel<SkillResponseModel>> UpdateSkill(Guid skillId,  UpdateSkillRequestModel request);
    Task<BaseModel> DeleteSkill(Guid skillId);
   
}