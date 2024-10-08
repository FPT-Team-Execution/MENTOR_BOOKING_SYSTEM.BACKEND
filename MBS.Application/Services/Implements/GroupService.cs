using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Groups;
using MBS.Application.Models.Project;
using MBS.Application.Services.Interfaces;
using MBS.Core.Entities;
using MBS.DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Services.Implements
{
    public class GroupService : BaseService<GroupService>, IGroupService
    {
        public GroupService(IUnitOfWork unitOfWork, ILogger<GroupService> logger, IMapper mapper) : base(unitOfWork, logger, mapper)
        {

        }

        public  async Task<BaseModel<CreateNewGroupResponseModel, CreateNewGroupRequestModel>> CreateNewGroupAsync(CreateNewGroupRequestModel request)
        {
            var newGroup = new Group()
            {
                Id = new Guid(),
                ProjectId = request.projectId,
                StudentId = request.StudentId,
                PositionId = request.PositionId,

            };
            try
            {
                await _unitOfWork.GetRepository<Group>().InsertAsync(newGroup);
                if (await _unitOfWork.CommitAsync() > 0)
                    return new BaseModel<CreateNewGroupResponseModel, CreateNewGroupRequestModel>
                    {
                        Message = MessageResponseHelper.CreateSuccessfully("group"),
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                        RequestModel = request,
                        ResponseModel = new CreateNewGroupResponseModel
                        {
                            newGroupResponse = newGroup,
                        }
                    };
                return new BaseModel<CreateNewGroupResponseModel, CreateNewGroupRequestModel>
                {
                    Message = MessageResponseHelper.CreateFailed("group"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status200OK,
                    RequestModel = request,
                    ResponseModel = null,
                };
            }
            catch (Exception e)
            {
                return new BaseModel<CreateNewGroupResponseModel, CreateNewGroupRequestModel>
                {
                    Message = e.Message,
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }

        public async Task<BaseModel<GetAllGroupResponseModel, GetAllGroupRequestModel>> GetAllGroup()
        {
            try
            {
                var groupsList = await _unitOfWork.GetRepository<Group>().GetListAsync();
                if (await _unitOfWork.CommitAsync() > 0)
                    return new BaseModel<GetAllGroupResponseModel, GetAllGroupRequestModel>
                    {
                        Message = MessageResponseHelper.GetSuccessfully("list of groups"),
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                        RequestModel = null,
                        ResponseModel = new GetAllGroupResponseModel
                        {
                            groups = groupsList.ToList()                 
                        }
                    };
                return new BaseModel<GetAllGroupResponseModel, GetAllGroupRequestModel>
                {
                    Message = MessageResponseHelper.CreateFailed("group"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status200OK,
                    RequestModel = null,
                    ResponseModel = null,
                };
            }
            catch (Exception e)
            {
                return new BaseModel<GetAllGroupResponseModel, GetAllGroupRequestModel>
                {
                    Message = e.Message,
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }

        public async Task<BaseModel<GetGroupResponseModel, GetGroupRequestModel>> GetGroup(GetGroupRequestModel request)
        {
            var groupFindById = await _unitOfWork.GetRepository<Group>().SingleOrDefaultAsync(a => a.Id == request.Id);
            try
            {
                if (groupFindById != null)
                {
                    return new BaseModel<GetGroupResponseModel, GetGroupRequestModel>
                    {
                        Message = MessageResponseHelper.GetSuccessfully("group"),
                        IsSuccess = true,
                        StatusCode = StatusCodes.Status200OK,
                        RequestModel = request,
                        ResponseModel = new GetGroupResponseModel
                        {
                            groupResponse = groupFindById,
                        }
                    };
                }
                else
                {
                    return new BaseModel<GetGroupResponseModel, GetGroupRequestModel>
                    {
                        Message = MessageResponseHelper.GetFailed("group"),
                        IsSuccess = false,
                        StatusCode = StatusCodes.Status200OK,
                        RequestModel = request,
                        ResponseModel = null
                    };
                }
            }
            catch (Exception e)
            {
                return new BaseModel<GetGroupResponseModel, GetGroupRequestModel>
                {
                    Message = e.Message,
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
            }
        }

		public async Task<BaseModel<RemoveGroupResponseModel, RemoveGroupRequestModel>> RemoveGroup(RemoveGroupRequestModel request)
		{
			var groupFindById = await _unitOfWork.GetRepository<Group>().SingleOrDefaultAsync(a => a.Id == request.GroupId);
			try
			{
				if (groupFindById != null)
				{
					_unitOfWork.GetRepository<Group>().DeleteAsync(groupFindById);
					await _unitOfWork.CommitAsync();

					return new BaseModel<RemoveGroupResponseModel, RemoveGroupRequestModel>
					{
						Message = MessageResponseHelper.DeleteSuccessfully("Group"),
						IsSuccess = true,
						StatusCode = StatusCodes.Status200OK,
						RequestModel = request,
						ResponseModel = new RemoveGroupResponseModel { Success = true }
					};
				}
				else
				{
					return new BaseModel<RemoveGroupResponseModel, RemoveGroupRequestModel>
					{
						Message = MessageResponseHelper.DeleteFailed("Group"),
						IsSuccess = false,
						StatusCode = StatusCodes.Status404NotFound,
						RequestModel = request,
						ResponseModel = new RemoveGroupResponseModel { Success = false }
					};
				}
			}
			catch (Exception e)
			{
				return new BaseModel<RemoveGroupResponseModel, RemoveGroupRequestModel>
				{
					Message = e.Message,
					IsSuccess = false,
					StatusCode = StatusCodes.Status500InternalServerError,
				};
			}
		}


		public async Task<BaseModel<UpdateGroupResponseModel, UpdateGroupRequestModel>> UpdateGroup(Guid id, UpdateGroupRequestModel request)
		{
			var groupFindById = await _unitOfWork.GetRepository<Group>().SingleOrDefaultAsync(a => a.Id == id);  
			try
			{
				if (groupFindById != null)
				{
					groupFindById.StudentId = request.studentId;
					groupFindById.PositionId = request.PositionId;
					_unitOfWork.GetRepository<Group>().UpdateAsync(groupFindById);
					if (await _unitOfWork.CommitAsync() > 0)
					{
						return new BaseModel<UpdateGroupResponseModel, UpdateGroupRequestModel>
						{
							Message = MessageResponseHelper.UpdateSuccessfully("Group"),
							IsSuccess = true,
							StatusCode = StatusCodes.Status200OK,
							RequestModel = request,
							ResponseModel = new UpdateGroupResponseModel
							{
								updatedGroup = groupFindById,
							}
						};
					}
					else
					{
						return new BaseModel<UpdateGroupResponseModel, UpdateGroupRequestModel>
						{
							Message = MessageResponseHelper.UpdateFailed("Group"),
							IsSuccess = false,
							StatusCode = StatusCodes.Status500InternalServerError,
							RequestModel = request,
							ResponseModel = null
						};
					}
				}
				else
				{
					return new BaseModel<UpdateGroupResponseModel, UpdateGroupRequestModel>
					{
						Message = MessageResponseHelper.Fail("Group not found"),
						IsSuccess = false,
						StatusCode = StatusCodes.Status404NotFound,
						RequestModel = request,
						ResponseModel = null
					};
				}
			}
			catch (Exception e)
			{
				return new BaseModel<UpdateGroupResponseModel, UpdateGroupRequestModel>
				{
					Message = e.Message,
					IsSuccess = false,
					StatusCode = StatusCodes.Status500InternalServerError,
				};
			}
		}

	}
}
