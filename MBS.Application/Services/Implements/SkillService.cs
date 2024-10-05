using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Skill;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class SkillService : BaseService<SkillService>, ISkillService
{
    public SkillService(IUnitOfWork unitOfWork, ILogger<SkillService> logger) : base(unitOfWork, logger)
    {
    }

    public async Task<BaseModel<SkillResponseModel, CreateSkillRequestModel>> CreateSkill(CreateSkillRequestModel request)
    {
        try
        {
            //check mentor
            var mentor = _unitOfWork.GetRepository<Mentor>().SingleOrDefaultAsync(x => x.UserId == request.MentorId);
            if(mentor == null)
                return new BaseModel<SkillResponseModel, CreateSkillRequestModel>
                {
                    Message = MessageResponseHelper.UserNotFound(request.MentorId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            //Create meeting
            var newSkill = new Skill()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                MentorId = request.MentorId,
            };
            await _unitOfWork.GetRepository<Skill>().InsertAsync(newSkill);
            if(await _unitOfWork.CommitAsync() > 0)
                return new BaseModel<SkillResponseModel, CreateSkillRequestModel>
                {
                    Message = MessageResponseHelper.GetSuccessfully("skill"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    RequestModel = request,
                    ResponseModel = new SkillResponseModel
                    {
                        Skill = newSkill
                    }
                };
            return new BaseModel<SkillResponseModel, CreateSkillRequestModel>
            {
                Message = MessageResponseHelper.CreateFailed("skill"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status200OK,
            };
        }
        catch (Exception e)
        {
            return new BaseModel<SkillResponseModel, CreateSkillRequestModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<Pagination<Skill>>> GetSkillsByMentorId(string mentorId, int page, int size)
    {
        try
        {
            //check mentor
            var mentor = _unitOfWork.GetRepository<Mentor>().SingleOrDefaultAsync(x => x.UserId == mentorId);
            if (mentor == null)
                return new BaseModel<Pagination<Skill>>
                {
                    Message = MessageResponseHelper.UserNotFound(mentorId),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            var skills = await _unitOfWork.GetRepository<Skill>().GetPagingListAsync(
                predicate: s => s.MentorId == mentorId,
                page: page,
                size: size);

            return new BaseModel<Pagination<Skill>>()
            {
                Message = MessageResponseHelper.Successfully("Get all " + nameof(Skill)),
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = true,
                ResponseRequestModel = skills,
            };
        }
        catch (Exception e)
        {
            return new BaseModel<Pagination<Skill>>()
            {
                Message = e.Message,
                StatusCode = StatusCodes.Status500InternalServerError,
                IsSuccess = false,
            };
        }
    }

    public async Task<BaseModel<Pagination<Skill>>> Getskills(int page, int size)
    {
        try
        {
            var skills = await _unitOfWork.GetRepository<Skill>().GetPagingListAsync(
                page: page,
                size: size);

            return new BaseModel<Pagination<Skill>>()
            {
                Message = MessageResponseHelper.Successfully("Get all " + nameof(Major)),
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = true,
                ResponseRequestModel = skills,
            };
        }
        catch (Exception e)
        {
            return new BaseModel<Pagination<Skill>>()
            {
                Message = e.Message,
                StatusCode = StatusCodes.Status500InternalServerError,
                IsSuccess = false,
            };
        }
    }

    public async Task<BaseModel<SkillResponseModel>> GetSkillById(Guid skillId)
    {
        try
        {
            var skill = await _unitOfWork.GetRepository<Skill>().SingleOrDefaultAsync(i => i.Id == skillId);
            if (skill == null) 
            {
                return new BaseModel<SkillResponseModel>()
                {
                    Message = MessageResponseHelper.Fail("Get " + nameof(Skill)),
                    StatusCode = StatusCodes.Status404NotFound,
                    IsSuccess = false,
                };
            }
            return new BaseModel<SkillResponseModel>()
            {
                Message = MessageResponseHelper.Successfully("Get " + nameof(Major)),
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = true,
                ResponseRequestModel = new SkillResponseModel
                {
                    Skill = skill
                },
            };
        }
        catch (Exception e)
        {
            return new BaseModel<SkillResponseModel>()
            {
                Message = e.Message,
                StatusCode = StatusCodes.Status500InternalServerError,
                IsSuccess = false,
            };
        }
    }

    public async Task<BaseModel<SkillResponseModel>> UpdateSkill(Guid skillId, UpdateSkillRequestModel request)
    {
        try
        {
            var skill = await _unitOfWork.GetRepository<Skill>().SingleOrDefaultAsync(i => i.Id == skillId);
            if (skill == null)
            {
                return new BaseModel<SkillResponseModel>()
                {
                    Message = MessageResponseHelper.Fail("Update " + nameof(Skill)),
                    StatusCode = StatusCodes.Status404NotFound,
                    IsSuccess = false,
                };
            }

            skill.Name = request.Name;
            _unitOfWork.GetRepository<Skill>().UpdateAsync(skill);
            return new BaseModel<SkillResponseModel>()
            {
                Message = MessageResponseHelper.Successfully("Update " + nameof(Major)),
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = true,
                ResponseRequestModel = new SkillResponseModel()
                {
                    Skill = skill
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<SkillResponseModel>()
            {
                Message = e.Message,
                StatusCode = StatusCodes.Status500InternalServerError,
                IsSuccess = false,
            };
        }
    }

    public async Task<BaseModel> DeleteSkill(Guid skillId)
    {
        try
        {
            var skill = await _unitOfWork.GetRepository<Skill>().SingleOrDefaultAsync(i => i.Id == skillId);
            if (skill == null)
            {
                return new BaseModel()
                {
                    Message = MessageResponseHelper.DeleteFailed("major"),
                    StatusCode = StatusCodes.Status404NotFound,
                    IsSuccess = false,
                };
            }

            _unitOfWork.GetRepository<Skill>().DeleteAsync(skill);
            if (_unitOfWork.Commit() > 0)
                return new BaseModel()
                {
                    Message = MessageResponseHelper.Successfully("Delete " + nameof(skill)),
                    StatusCode = StatusCodes.Status200OK,
                    IsSuccess = true,
                };
            return new BaseModel()
            {
                Message = MessageResponseHelper.DeleteFailed("skill"),
                StatusCode = StatusCodes.Status204NoContent,
                IsSuccess = false,
            };
        }
        catch (Exception e)
        {
            return new BaseModel()
            {
                Message = e.Message,
                StatusCode = StatusCodes.Status500InternalServerError,
                IsSuccess = false,
            };
        }
    }
}