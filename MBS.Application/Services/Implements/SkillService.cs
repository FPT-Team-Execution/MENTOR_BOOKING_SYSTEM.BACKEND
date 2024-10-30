using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Skill;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.Repositories;
using MBS.DataAccess.Repositories.Implements;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class SkillService : BaseService2<SkillService>, ISkillService
{
    private readonly ISkillRepository _skillRepository;
    private readonly IMentorRepository _mentorRepository;

    public SkillService(ILogger<SkillService> logger, IMapper mapper,
        ISkillRepository skillRepository, IMentorRepository mentorRepository
    ) : base(logger, mapper)
    {
        _skillRepository = skillRepository;
        _mentorRepository = mentorRepository;
    }

    public async Task<BaseModel<SkillResponseModel, CreateSkillRequestModel>> CreateSkill(CreateSkillRequestModel request)
    {
        try
        {
            var newSkill = new Skill()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                MentorId = request.MentorId,
            };
            var createResult = await _skillRepository.CreateAsync(newSkill);
            if(createResult)
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
            var skills = await _skillRepository.GetPagedListAsyncByMentorId(page, size, mentorId);

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

    


    public async Task<BaseModel<SkillResponseModel>> GetSkillById(Guid skillId)
    {
        try
        {
            var skill = await _skillRepository.GetByIdAsync(skillId, "Id");
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
            var skill = await _skillRepository.GetByIdAsync(skillId, "Id");
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
            var updateResult = _skillRepository.Update(skill); 
            if(updateResult)
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
            return new BaseModel<SkillResponseModel>()
            {
                Message = MessageResponseHelper.UpdateFailed("skill"),
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = false,
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
            var skill = await _skillRepository.GetByIdAsync(skillId, "Id");
            if (skill == null)
            {
                return new BaseModel()
                {
                    Message = MessageResponseHelper.DeleteFailed("major"),
                    StatusCode = StatusCodes.Status404NotFound,
                    IsSuccess = false,
                };
            }

            var deleteResult = _skillRepository.Delete(skill);
            if (deleteResult)
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

    public async Task<BaseModel<Pagination<SkillSummaryResponseDTO>>> Getskills(int page, int size)
    {
        try
        {
            var result = await _skillRepository.GetPagedListAsync(page: page, size: size);
            var skillDTOList = new List<SkillSummaryResponseDTO>();

            foreach (var item in result.Items)
            {
                var skillFound = await _skillRepository.GetSkillByIdAsync(item.Id);
                var mentorBySkill = await _mentorRepository.GetMentorByIdAsync(skillFound.MentorId);
                var skillDTO = new SkillSummaryResponseDTO
                {
                    Name = skillFound.Name,
                    MentorName = mentorBySkill.User.FullName,
                    MentorEmail = mentorBySkill.User.Email

                };
                skillDTOList.Add(skillDTO);
            }

            var paginatedSkill = new Pagination<SkillSummaryResponseDTO>
            {
                Items = skillDTOList,
                PageSize = size,
                PageIndex = page
            };

            return new BaseModel<Pagination<SkillSummaryResponseDTO>>
            {
                Message = MessageResponseHelper.Successfully("Get all " + nameof(Skill)),
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = true,
                ResponseRequestModel = paginatedSkill
            };
        }
        catch (Exception e)
        {
            return new BaseModel<Pagination<SkillSummaryResponseDTO>>
            {
                Message = e.Message,
                StatusCode = StatusCodes.Status500InternalServerError,
                IsSuccess = false
            };
        }
    }

}