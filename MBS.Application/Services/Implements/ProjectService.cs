using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Project;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess.DAO;
using MBS.DataAccess.Repositories;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class ProjectService : BaseService2<ProjectService>, IProjectService
{
	private readonly IProjectRepository _projectRepository;
	private readonly IGroupRepository _groupRepository;
	public ProjectService(
		IGroupRepository groupRepository,
		IProjectRepository projectRepository,
		ILogger<ProjectService> logger, IMapper mapper) : base(logger, mapper)
	{
		_groupRepository = groupRepository;
		_projectRepository = projectRepository;
	}
	public async Task<BaseModel<CreateProjectResponseModel, CreateProjectRequestModel>> CreateProject(CreateProjectRequestModel request)
	{
		var projectCreate = new Project
		{
			Id = Guid.NewGuid(),
			Title = request.Title,
			Description = request.Description,
			DueDate = request.DueDate,
			Semester = request.Semester,
			MentorId = request.MentorId,
			Status = ProjectStatusEnum.Activated
		};
		try
		{
			var createResult = await _projectRepository.CreateAsync(projectCreate);
			if (createResult)
				return new BaseModel<CreateProjectResponseModel, CreateProjectRequestModel>
				{
					Message = MessageResponseHelper.CreateSuccessfully("project"),
					IsSuccess = true,
					StatusCode = StatusCodes.Status200OK,
					RequestModel = request,
					ResponseModel = new CreateProjectResponseModel
					{
						ProjectId = projectCreate.Id
					}
				};
			return new BaseModel<CreateProjectResponseModel, CreateProjectRequestModel>
			{
				Message = MessageResponseHelper.CreateFailed("project"),
				IsSuccess = false,
				StatusCode = StatusCodes.Status200OK,
				RequestModel = request,
				ResponseModel = new CreateProjectResponseModel
				{
					ProjectId = projectCreate.Id
				}
			};
		}
		catch (Exception e)
		{
			return new BaseModel<CreateProjectResponseModel, CreateProjectRequestModel>
			{
				Message = e.Message,
				IsSuccess = false,
				StatusCode = StatusCodes.Status500InternalServerError,
			};
		}
	}

	public async Task<BaseModel<Pagination<ProjectResponseDto>>> GetProjectsByStudentId(GetProjectsByStudentIdRequest request)
{
    try
    {
        // Initialize an empty list of projects
        List<Project> enrolledProjects = new List<Project>();
        
        // Fetch groups that the student is enrolled in, along with pagination
        var enrolledProjectGroups = await _groupRepository.GetGroupsByStudentId(
            request.StudentId, 
            request.Page, 
            request.Size,
            request.SortOrder
        );
        
        // Check if no groups were found
        if (!enrolledProjectGroups.Items.Any())
        {
            return new BaseModel<Pagination<ProjectResponseDto>>
            {
                Message = MessageResponseHelper.GetSuccessfully("projects"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status204NoContent,
                ResponseRequestModel = new Pagination<ProjectResponseDto>
                {
                    Items = new List<ProjectResponseDto>(),
                    PageIndex = request.Page,
                    PageSize = request.Size,
                    TotalPages = 0
                }
            };
        }

        // Filter projects based on the presence of ProjectStatus
        enrolledProjects = request.ProjectStatus == null
            ? enrolledProjectGroups.Items.Select(g => g.Project).ToList() // Get all projects
            : enrolledProjectGroups.Items.Select(g => g.Project)
                                         .Where(p => p.Status == request.ProjectStatus)
                                         .ToList(); // Get projects by status

        // Prepare the response DTO for the projects
        var projectDtos = _mapper.Map<List<ProjectResponseDto>>(enrolledProjects);

        return new BaseModel<Pagination<ProjectResponseDto>>
        {
            Message = MessageResponseHelper.GetSuccessfully("projects"),
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            ResponseRequestModel = new Pagination<ProjectResponseDto>
            {
                Items = projectDtos,
                PageIndex = request.Page,
                PageSize = request.Size,
            }
        };
    }
    catch (Exception e)
    {
        return new BaseModel<Pagination<ProjectResponseDto>>
        {
            Message = e.Message,
            IsSuccess = false,
            StatusCode = StatusCodes.Status500InternalServerError,
        };
    }
}


	public async Task<BaseModel<ProjectResponseModel>> UpdateProject(Guid projectId, UpdateProjectRequestModel request)
	{
		try
		{
			if (request.DueDate <= DateTime.Now)
				return new BaseModel<ProjectResponseModel>
				{
					Message = MessageResponseHelper.InvalidInputParameter(),
					IsSuccess = false,
					StatusCode = StatusCodes.Status400BadRequest,
				};
			var projectUpdate = await _projectRepository.GetByIdAsync(projectId, "Id");
			if (projectUpdate == null)
				return new BaseModel<ProjectResponseModel>
				{
					Message = MessageResponseHelper.ProjectNotFound(projectId.ToString()),
					IsSuccess = false,
					StatusCode = StatusCodes.Status404NotFound,
				};
			//* update fields
			projectUpdate.Title = request.Title;
			projectUpdate.Description = request.Description;
			projectUpdate.DueDate = request.DueDate;
			projectUpdate.Semester = request.Semester;
			projectUpdate.Status = request.Status;
			var updateResult = _projectRepository.Update(projectUpdate);
			if (updateResult)
				return new BaseModel<ProjectResponseModel>
				{
					Message = MessageResponseHelper.UpdateSuccessfully("project"),
					IsSuccess = true,
					StatusCode = StatusCodes.Status200OK,
					ResponseRequestModel = new ProjectResponseModel
					{
						Project = _mapper.Map<ProjectResponseDto>(projectUpdate),
					}
				};

			return new BaseModel<ProjectResponseModel>
			{
				Message = MessageResponseHelper.UpdateFailed("project"),
				IsSuccess = false,
				StatusCode = StatusCodes.Status500InternalServerError,
			};
		}
		catch (Exception e)
		{
			return new BaseModel<ProjectResponseModel>
			{
				Message = e.Message,
				IsSuccess = false,
				StatusCode = StatusCodes.Status500InternalServerError,
			};
		}
	}

	// public async Task<BaseModel<ProjectResponseModel>> UpdateProjectStatus(Guid projectId, ProjectStatusEnum newStatus)
	// {
	// 	try
	// 	{
	// 		var projectUpdate = await _projectRepository.GetByIdAsync(projectId, "Id");
	//
	// 		if (projectUpdate == null)
	// 			return new BaseModel<ProjectResponseModel>
	// 			{
	// 				Message = MessageResponseHelper.ProjectNotFound(projectId.ToString()),
	// 				IsSuccess = false,
	// 				StatusCode = StatusCodes.Status404NotFound,
	// 			};
	// 		//not allow to updated if this project closed
	// 		if (projectUpdate.Status == ProjectStatusEnum.Closed)
	// 		{
	// 			return new BaseModel<ProjectResponseModel>
	// 			{
	// 				Message = MessageResponseHelper.ProjectClosed(projectId.ToString()),
	// 				IsSuccess = false,
	// 				StatusCode = StatusCodes.Status400BadRequest,
	// 			};
	// 		}
	// 		//* update status
	// 		projectUpdate.Status = newStatus;
	// 		_unitOfWork.GetRepository<Project>().UpdateAsync(projectUpdate);
	// 		return new BaseModel<ProjectResponseModel>
	// 		{
	// 			Message = MessageResponseHelper.UpdateSuccessfully("project"),
	// 			IsSuccess = true,
	// 			StatusCode = StatusCodes.Status200OK,
	// 			ResponseRequestModel = new ProjectResponseModel
	// 			{
	// 				Project = _mapper.Map<ProjectResponseDto>(projectUpdate)
	// 			}
	// 		};
	// 	}
	// 	catch (Exception e)
	// 	{
	// 		return new BaseModel<ProjectResponseModel>
	// 		{
	// 			Message = e.Message,
	// 			IsSuccess = false,
	// 			StatusCode = StatusCodes.Status500InternalServerError,
	// 		};
	// 	}
	// }

	public async Task<BaseModel<ProjectResponseModel>> GetProjectById(Guid projectId)
	{
		try
		{
			var project = await _projectRepository.GetByIdAsync(projectId, "Id");

			if (project == null)
				return new BaseModel<ProjectResponseModel>
				{
					Message = MessageResponseHelper.ProjectNotFound(projectId.ToString()),
					IsSuccess = false,
					StatusCode = StatusCodes.Status404NotFound,
				};
			return new BaseModel<ProjectResponseModel>
			{
				Message = MessageResponseHelper.GetSuccessfully("project"),
				IsSuccess = true,
				StatusCode = StatusCodes.Status200OK,
				ResponseRequestModel = new ProjectResponseModel
				{
					Project = _mapper.Map<ProjectResponseDto>(project)
				}
			};
		}
		catch (Exception e)
		{
			return new BaseModel<ProjectResponseModel>
			{
				Message = e.Message,
				IsSuccess = false,
				StatusCode = StatusCodes.Status500InternalServerError,
			};
		}
	}

	public async Task<BaseModel<AssignMentorResponseModel>> AssignMentor(Guid projectId, string mentorId)
	{
		try
		{
			var project = await _projectRepository.GetByIdAsync(projectId, "Id");

			if (project == null)
				return new BaseModel<AssignMentorResponseModel>
				{
					Message = MessageResponseHelper.ProjectNotFound(projectId.ToString()),
					IsSuccess = false,
					StatusCode = StatusCodes.Status404NotFound,
				};
			//can not assign mentor to project which is not actived
			if (project.Status != ProjectStatusEnum.Activated)
			{
				return new BaseModel<AssignMentorResponseModel>
				{
					Message = MessageResponseHelper.ProjectNotActivated(projectId.ToString()),
					IsSuccess = false,
					StatusCode = StatusCodes.Status400BadRequest,
				};
			}
			//* update status
			project.MentorId = mentorId;
			var updateResult = _projectRepository.Update(project);
			if(updateResult)
				return new BaseModel<AssignMentorResponseModel>
				{
					Message = MessageResponseHelper.UpdateSuccessfully("project"),
					IsSuccess = true,
					StatusCode = StatusCodes.Status200OK,
					ResponseRequestModel = new AssignMentorResponseModel
					{
						Project = _mapper.Map<ProjectResponseDto>(project),
					}
				};
			return new BaseModel<AssignMentorResponseModel>
			{
				Message = MessageResponseHelper.UpdateFailed("project"),
				IsSuccess = false,
				StatusCode = StatusCodes.Status500InternalServerError,
			};
		}
		catch (Exception e)
		{
			return new BaseModel<AssignMentorResponseModel>
			{
				Message = e.Message,
				IsSuccess = false,
				StatusCode = StatusCodes.Status500InternalServerError,
			};
		}
	}
}