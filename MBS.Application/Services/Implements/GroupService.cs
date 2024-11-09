using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Groups;
using MBS.Application.Services.Interfaces;
using MBS.Core.Entities;
using MBS.DataAccess.DAO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MBS.Core.Common.Pagination;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MBS.Core.Enums;


namespace MBS.Application.Services.Implements
{
	public class GroupService : BaseService2<GroupService>, IGroupService
	{
		private readonly IGroupRepository _groupRepository;
		private readonly IStudentRepository _studentRepository;
		private readonly IMajorRepository _majorRepository;
		private readonly IProjectRepository _projectRepository;
		private readonly IPointTransactionSerivce _pointTransactionSerivce;

		public GroupService(
			IMajorRepository majorRepository,
			IProjectRepository projectRepository,
			IGroupRepository groupRepository,
			IStudentRepository studentRepository,
			ILogger<GroupService> logger, IMapper mapper)
			: base(logger, mapper)
		{
			_groupRepository = groupRepository;
			_studentRepository = studentRepository;
			_majorRepository = majorRepository;
			_projectRepository = projectRepository;
		}

		public async Task<BaseModel<CreateNewGroupResponseModel, CreateNewGroupRequestModel>> CreateNewGroupAsync(CreateNewGroupRequestModel request)
		{
			var groupExist = await _groupRepository.GetGroupByProjectAndStudentIdAsync(request.ProjectId, request.StudentId);
			if (groupExist != null)
			{
				return new BaseModel<CreateNewGroupResponseModel, CreateNewGroupRequestModel>
				{
					Message = MessageResponseHelper.CreateFailed("group"),
					IsSuccess = false,
					StatusCode = StatusCodes.Status200OK,
					RequestModel = request,
					ResponseModel = null
				};
			}
			var newGroup = new Group
			{
				Id = Guid.NewGuid(),
				ProjectId = request.ProjectId,
				StudentId = request.StudentId,
				PositionId = request.PositionId
			};

			await _groupRepository.CreateAsync(newGroup);

			await _pointTransactionSerivce.ModifyStudentPoint(new Models.PointTransaction.ModifyStudentPointRequestModel()
			{
				Amount = 400,
				StudentId = request.StudentId,
				TransactionType = nameof(TransactionTypeEnum.Credit)
			});

			return new BaseModel<CreateNewGroupResponseModel, CreateNewGroupRequestModel>
			{
				Message = MessageResponseHelper.CreateSuccessfully("group"),
				IsSuccess = true,
				StatusCode = StatusCodes.Status200OK,
				RequestModel = request,
				ResponseModel = new CreateNewGroupResponseModel
				{
					Id = newGroup.Id,
				}
			};
		}

		public async Task<BaseModel<GroupModel>> GetGroupId(Guid requestId)
		{
			try
			{
				var group = await _groupRepository.GetByIdAsync(requestId, "Id");
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
			catch (Exception e)
			{
				return new BaseModel<GroupModel>
				{
					Message = e.Message,
					IsSuccess = false,
					StatusCode = StatusCodes.Status500InternalServerError
				};
			}
		}
		public async Task<BaseModel<GroupModel>> UpdateGroup(Guid id, UpdateGroupRequestModel request)
		{
			var group = await _groupRepository.GetByIdAsync(id, "Id");
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

			var updateRs = _groupRepository.Update(group);
			if (!updateRs)
				return new BaseModel<GroupModel>
				{
					Message = MessageResponseHelper.UpdateFailed("group"),
					IsSuccess = false,
					StatusCode = StatusCodes.Status500InternalServerError,
				};
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
			var group = await _groupRepository.GetByIdAsync(id, "Id");
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
			var updateRs = _groupRepository.Update(group);
			if (!updateRs)
				return new BaseModel
				{
					Message = MessageResponseHelper.DeleteFailed("group"),
					IsSuccess = false,
					StatusCode = StatusCodes.Status500InternalServerError,
				};

			await _pointTransactionSerivce.ModifyStudentPoint(new Models.PointTransaction.ModifyStudentPointRequestModel()
			{
				Amount = 400,
				StudentId = group.StudentId,
				TransactionType = nameof(TransactionTypeEnum.Debit)
			});

			return new BaseModel
			{
				Message = MessageResponseHelper.DeleteSuccessfully("group"),
				IsSuccess = true,
				StatusCode = StatusCodes.Status200OK
			};
		}

		public async Task<BaseModel<Pagination<GroupResponseDTO>>> GetGroups(int page, int size)
		{
			var result = await _groupRepository.GetPagedListBaseAsync(page: page, size: size);

			var groupDtoList = new List<GroupResponseDTO>();

			foreach (var group in result.Items)
			{
				var studentFound = await _studentRepository.GetByUserIdAsync(group.StudentId, include: m => m.Include(t => t.User));
				var groupDto = new GroupResponseDTO
				{
					ProjectName = group.Project.Title,
					StudentName = studentFound.User.FullName,
					PositionName = group.Position.Name
				};
				groupDtoList.Add(groupDto);
			}

			var paginatedDtoList = new Pagination<GroupResponseDTO>
			{
				Items = groupDtoList,
				PageIndex = page,
				PageSize = size,
				TotalItems = result.TotalItems,
				TotalPages = result.TotalPages

			};

			return new BaseModel<Pagination<GroupResponseDTO>>
			{
				Message = MessageResponseHelper.GetSuccessfully("groups"),
				IsSuccess = true,
				StatusCode = StatusCodes.Status200OK,
				ResponseRequestModel = paginatedDtoList
			};
		}


		public async Task<BaseModel<GroupStudentsResponseDTO>> GetStudentsInGroupByProjectId(Guid projectId)
		{
			var groupFound = await _groupRepository.GetGroupByProjectIdAsync(projectId);
			var response = new GroupStudentsResponseDTO();
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
							FullName = student.User.FullName,
							Email = student.User.Email,
							University = student.University,
							Major = await _majorRepository.GetMajorByIdAsync(student.MajorId),
							WalletPoint = student.WalletPoint,
						});
					}
				}

				response = new GroupStudentsResponseDTO
				{
					Project = await _projectRepository.GetByIdAsync(projectId, "Id"),
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
			response.Project = await _projectRepository.GetByIdAsync(projectId, "Id");

			return new BaseModel<GroupStudentsResponseDTO>
			{
				Message = MessageResponseHelper.GetFailed("groups"),
				IsSuccess = false,
				StatusCode = StatusCodes.Status200OK,
				ResponseRequestModel = response
			};
		}

		public async Task<BaseModel<List<StudentSearchDTO>>> SearchStudent(string searchItem)
		{
			searchItem = searchItem.ToLower();
			var students = await _studentRepository.GetStudents();
			List<StudentSearchDTO> studentSearchDTOs = new List<StudentSearchDTO>();

			if (students != null && students.Any())
			{
				foreach (var student in students)
				{
					var searchUser =
						await _studentRepository.GetByUserIdAsync(student.UserId, m => m.Include(x => x.User));

					string fullName = searchUser.User.FullName.ToLower();
					string email = searchUser.User.Email.ToLower();

					if (fullName.Contains(searchItem) || email.Contains(searchItem))
					{
						studentSearchDTOs.Add(new StudentSearchDTO
						{
							StudentId = student.UserId,
							FullName = searchUser.User.FullName,
							Email = searchUser.User.Email,
							Major = student.Major,
							University = student.University,
							WalletPoint = student.WalletPoint
						});
					}
				}
			}

			var response = studentSearchDTOs;
			return new BaseModel<List<StudentSearchDTO>>
			{
				Message = MessageResponseHelper.GetSuccessfully(""),
				IsSuccess = true,
				StatusCode = StatusCodes.Status200OK,
				ResponseRequestModel = response
			};
		}

		public async Task<BaseModel> RemoveStudentInGroupByProjectAndStudentIdAsync(Guid projectId, string studentId)
		{
			var memberFound = await _groupRepository.GetGroupByProjectAndStudentIdAsync(projectId, studentId);
			if (memberFound != null)
			{
				_groupRepository.Delete(memberFound);
				return new BaseModel
				{
					IsSuccess = true,
					Message = "Remove successfully",
					StatusCode = StatusCodes.Status200OK

				};
			}

			await _pointTransactionSerivce.ModifyStudentPoint(new Models.PointTransaction.ModifyStudentPointRequestModel()
			{
				Amount = 400,
				StudentId = memberFound.StudentId,
				TransactionType = nameof(TransactionTypeEnum.Debit)
			});

			return new BaseModel
			{
				IsSuccess = false,
				Message = "Remove false",
				StatusCode = StatusCodes.Status200OK
			};
		}
	}
}