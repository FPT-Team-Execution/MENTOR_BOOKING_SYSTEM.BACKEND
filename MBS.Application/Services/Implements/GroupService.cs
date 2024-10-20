using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Groups;
using MBS.Application.Services.Interfaces;
using MBS.Core.Entities;
using MBS.DataAccess.DAO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using MBS.Core.Common.Pagination;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace MBS.Application.Services.Implements
{
    public class GroupService : BaseService<GroupService>, IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IMajorRepository _majorRepository;
        private readonly IProjectRepository _projectRepository;


        public GroupService(IUnitOfWork unitOfWork, IMajorRepository majorRepository, IProjectRepository projectRepository, IGroupRepository groupRepository, ILogger<GroupService> logger, IMapper mapper, IStudentRepository studentRepository)
            : base(unitOfWork, logger, mapper)
        {
            _groupRepository = groupRepository; 
            _studentRepository = studentRepository;
            _majorRepository = majorRepository;
            _projectRepository = projectRepository;

        }

        public async Task<BaseModel<CreateNewGroupResponseModel, CreateNewGroupRequestModel>> CreateNewGroupAsync(CreateNewGroupRequestModel request)
        {
            var newGroup = new Group
            {
                Id = Guid.NewGuid(),
                ProjectId = request.projectId,
                StudentId = request.StudentId,
                PositionId = request.PositionId
            };

           await _groupRepository.CreateAsync(newGroup);

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
            var group = await _groupRepository.GetGroupByIdAsync(requestId);
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
            var group = await _groupRepository.GetGroupByIdAsync(id);
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

            _groupRepository.Update(group);

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
            var group = await _groupRepository.GetGroupByIdAsync(id);
            if (group == null)
            {
                return new BaseModel
                {
                    Message = MessageResponseHelper.DeleteFailed("group"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            if (group.StudentId == null)
            {
                _groupRepository.Delete(group);
                return new BaseModel
                {
                    Message = MessageResponseHelper.DeleteSuccessfully("group"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK
                };
            }
            return new BaseModel
            {
                Message = MessageResponseHelper.DeleteFailed("group"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK
            };

        }

        public async Task<BaseModel<Pagination<GroupResponseDTO>>> GetGroups(int page, int size)
        {
            var result = await _groupRepository.GetPagedListAsync(page, size);

            return new BaseModel<Pagination<GroupResponseDTO>>
            {
                Message = MessageResponseHelper.GetSuccessfully("groups"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = _mapper.Map<Pagination<GroupResponseDTO>>(result)
            };
        }

        public async Task<BaseModel<GroupStudentsResponseDTO>> GetStudentsInGroupByProjectId(Guid projectId)
        {
            var groupFound = await _groupRepository.GetGroupByProjectIdAsync(projectId);
            if (groupFound != null && groupFound.Any())
            {
                List<StudentInGroupDTO> studentDTOs = new List<StudentInGroupDTO>();

                foreach (var group in groupFound)
                {
                    Student student = await _studentRepository.GetByUserIdAsync(group.StudentId, m => m.Include(x => x.User));
                    if (student != null)
                    {
                        studentDTOs.Add(new StudentInGroupDTO
                        {
                            StudentId = student.UserId,
                            StudentName = student.User.FullName,
                            Email = student.User.Email,
                            University = student.University,
                            Major = await _majorRepository.GetMajorByIdAsync(student.MajorId),
                            walletPoint = student.WalletPoint,
                            
                        });
                    }
                }

                var response = new GroupStudentsResponseDTO
                {
                    Project = await _projectRepository.GetProjectById(projectId),
                    Students = studentDTOs
                };

                return new BaseModel<GroupStudentsResponseDTO>
                {
                    Message = MessageResponseHelper.GetSuccessfully("groups"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    ResponseRequestModel = response
                };
            }

            return new BaseModel<GroupStudentsResponseDTO>
            {
                Message = MessageResponseHelper.GetFailed("groups"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status404NotFound,
                ResponseRequestModel = null
            };
        }

    }
}
