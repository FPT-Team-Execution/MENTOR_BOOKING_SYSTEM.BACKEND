using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Project;
using MBS.Application.Services.Interfaces;
using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess.Repositories;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class ProjectService : BaseService<ProjectService>, IProjectService
{

    public ProjectService(IUnitOfWork unitOfWork, ILogger<ProjectService> logger) :base(unitOfWork, logger)
    {

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
            await _unitOfWork.GetRepository<Project>().InsertAsync(projectCreate);
            if(await _unitOfWork.CommitAsync() > 0)
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

    public async Task<BaseModel<GetProjectsByStudentIdResponseModel>> GetProjectsByStudentId(string studentId, ProjectStatusEnum? projectStatus, int page, int size)
    {
        try
        {
            List<Project> enrolledProjects =  [];
            var enrolledProjectProups = await _unitOfWork.GetRepository<Group>().GetPagingListAsync(
                predicate: g => g.StudentId == studentId,
                include: p => p.Include(x => x.Project),
                page: page,
                size: size
                );
            if(!enrolledProjectProups.Items.Any())
                return new BaseModel<GetProjectsByStudentIdResponseModel>
                {
                    Message = MessageResponseHelper.GetSuccessfully("projects"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    ResponseRequestModel = new GetProjectsByStudentIdResponseModel
                    {
                        Projects = enrolledProjects
                    }
                };
            enrolledProjects = projectStatus == null ? 
                //*project status null ~ get all projects of student    
                enrolledProjectProups.Items.Select(g => g.Project).ToList() 
                //*project status not null ~ get all projects of student base on project status
                : enrolledProjectProups.Items.Select(g => g.Project).Where(p => p.Status == projectStatus).ToList();
            
            return new BaseModel<GetProjectsByStudentIdResponseModel>
            {
                Message = MessageResponseHelper.GetSuccessfully("projects"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = new GetProjectsByStudentIdResponseModel
                {
                    Projects = enrolledProjects
                }
            };

        }
        catch (Exception e)
        {
            return new BaseModel<GetProjectsByStudentIdResponseModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
        
    }

    public async Task<BaseModel<UpdateProjectResponseModel>> UpdateProject(Guid projectId, UpdateProjectRequestModel request)
    {
        try
        {
            if (request.DueDate <= DateTime.Now)
                return new BaseModel<UpdateProjectResponseModel>
                {
                    Message = MessageResponseHelper.InvalidInputParameter(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            var projectUpdate = await _unitOfWork.GetRepository<Project>().SingleOrDefaultAsync(p => p.Id == projectId);
            if (projectUpdate == null)
                return new BaseModel<UpdateProjectResponseModel>
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
             _unitOfWork.GetRepository<Project>().UpdateAsync(projectUpdate);
            if (_unitOfWork.Commit() > 0)
                return new BaseModel<UpdateProjectResponseModel>
                {
                    Message = MessageResponseHelper.UpdateSuccessfully("project"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    ResponseRequestModel = new UpdateProjectResponseModel
                    {
                        Project = projectUpdate
                    }
                };
                
            return new BaseModel<UpdateProjectResponseModel>
            {
                Message = MessageResponseHelper.UpdateFailed("project"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status200OK,
            };
        }
        catch (Exception e)
        {
            return new BaseModel<UpdateProjectResponseModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<UpdateProjectStatusResponseModel>> UpdateProjectStatus(Guid projectId, ProjectStatusEnum newStatus)
    {
        try
        {
            var projectUpdate = await _unitOfWork.GetRepository<Project>().SingleOrDefaultAsync(p => p.Id == projectId);
            
            if(projectUpdate == null)
                return new BaseModel<UpdateProjectStatusResponseModel>
                {
                    Message = MessageResponseHelper.ProjectNotFound(projectId.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            //not allow to updated if this project closed
            if (projectUpdate.Status == ProjectStatusEnum.Closed)
            {
                return new BaseModel<UpdateProjectStatusResponseModel>
                {
                    Message = MessageResponseHelper.ProjectClosed(projectId.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            }
            //* update status
            projectUpdate.Status = newStatus;
            _unitOfWork.GetRepository<Project>().UpdateAsync(projectUpdate);
            return new BaseModel<UpdateProjectStatusResponseModel>
            {
                Message = MessageResponseHelper.UpdateSuccessfully("project"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel =new UpdateProjectStatusResponseModel
                {
                    Project = projectUpdate
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<UpdateProjectStatusResponseModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<GetProjectByIdResponseModel>> GetProjectById(Guid projectId)
    {
        try
        {
            var project = await _unitOfWork.GetRepository<Project>().SingleOrDefaultAsync(p => p.Id == projectId);
            
            if(project == null)
                return new BaseModel<GetProjectByIdResponseModel>
                {
                    Message = MessageResponseHelper.ProjectNotFound(projectId.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            return new BaseModel<GetProjectByIdResponseModel>
            {
                Message = MessageResponseHelper.GetSuccessfully("project"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = new GetProjectByIdResponseModel
                {
                    Project = project
                }
            };    
        }
        catch (Exception e)
        {
            return new BaseModel<GetProjectByIdResponseModel>
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
            var project = await _unitOfWork.GetRepository<Project>().SingleOrDefaultAsync(p => p.Id == projectId);
            
            if(project == null)
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
            _unitOfWork.GetRepository<Project>().UpdateAsync(project);
            return new BaseModel<AssignMentorResponseModel>
            {
                Message = MessageResponseHelper.UpdateSuccessfully("mentor"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel =new AssignMentorResponseModel
                {
                    Project = project
                }
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