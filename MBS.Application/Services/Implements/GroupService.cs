using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Groups;
using MBS.Application.Services.Interfaces;
using MBS.Core.Entities;
using MBS.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using MBS.Core.Common.Pagination;

namespace MBS.Application.Services.Implements
{
    public class GroupService : BaseService<GroupService>, IGroupService
    {
        public GroupService(IUnitOfWork unitOfWork, ILogger<GroupService> logger, IMapper mapper)
            : base(unitOfWork, logger, mapper) { }

        public async Task<BaseModel<CreateNewGroupResponseModel, CreateNewGroupRequestModel>> CreateNewGroupAsync(CreateNewGroupRequestModel request)
        {
            var newGroup = new Group
            {
                Id = Guid.NewGuid(),
                ProjectId = request.projectId,
                StudentId = request.StudentId,
                PositionId = request.PositionId
            };

            await _unitOfWork.GetRepository<Group>().InsertAsync(newGroup);
            await _unitOfWork.CommitAsync();

            return new BaseModel<CreateNewGroupResponseModel, CreateNewGroupRequestModel>
            {
                Message = MessageResponseHelper.CreateSuccessfully("group"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                RequestModel = request,
                ResponseModel = new CreateNewGroupResponseModel
                {
                    id = newGroup.Id,
                }
            };
        }

        public async Task<BaseModel<GroupModel>> GetGroupId(Guid requestId)
        {
            var group = await _unitOfWork.GetRepository<Group>().SingleOrDefaultAsync(g => g.Id == requestId);
            if (group == null)
            {
                return new BaseModel<GroupModel>
                {
                    Message = MessageResponseHelper.GetFailed("group"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            return new BaseModel<GroupModel>
            {
                Message = MessageResponseHelper.GetSuccessfully("group"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = new GroupModel
                {
                    groupResponseDTO = _mapper.Map<GroupResponseDTO>(group)
                }
            };
        }

        public async Task<BaseModel<GroupModel>> UpdateGroup(Guid id, UpdateGroupRequestModel request)
        {
            var group = await _unitOfWork.GetRepository<Group>().SingleOrDefaultAsync(g => g.Id == id);
            if (group == null)
            {
                return new BaseModel<GroupModel>
                {
                    Message = MessageResponseHelper.Fail("Group not found"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            group.StudentId = request.studentId;
            group.PositionId = request.PositionId;

            _unitOfWork.GetRepository<Group>().UpdateAsync(group);
            await _unitOfWork.CommitAsync();

            return new BaseModel<GroupModel>
            {
                Message = MessageResponseHelper.UpdateSuccessfully("group"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = new GroupModel
                {
                    groupResponseDTO = _mapper.Map<GroupResponseDTO>(group)
                }
            };
        }

        public async Task<BaseModel> RemoveGroup(Guid id)
        {
            var group = await _unitOfWork.GetRepository<Group>().SingleOrDefaultAsync(g => g.Id == id);
            if (group == null)
            {
                return new BaseModel
                {
                    Message = MessageResponseHelper.DeleteFailed("group"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            //group. = MBS.Core.Enums.StatusEnum.Deactivated;
            _unitOfWork.GetRepository<Group>().UpdateAsync(group);
            await _unitOfWork.CommitAsync();

            return new BaseModel
            {
                Message = MessageResponseHelper.DeleteSuccessfully("group"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK
            };
        }

        public async Task<BaseModel<Pagination<GroupResponseDTO>>> GetGroups(int page, int size)
        {
            var result = await _unitOfWork.GetRepository<Group>().GetPagingListAsync(page: page, size: size);

            return new BaseModel<Pagination<GroupResponseDTO>>
            {
                Message = MessageResponseHelper.GetSuccessfully("groups"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = _mapper.Map<Pagination<GroupResponseDTO>>(result)
            };
        }
    }
}
