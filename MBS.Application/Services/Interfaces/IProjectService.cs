using MBS.Application.Models.General;
using MBS.Application.Models.Project;
using MBS.Core.Enums;

namespace MBS.Application.Services.Interfaces;

public interface IProjectService
{
    Task<BaseModel<CreateProjectResponseModel, CreateProjectRequestModel>> CreateProject(CreateProjectRequestModel request);
    Task<BaseModel<GetAllProjectResponseModel>> GetProjectsByStudentId(string studentId, ProjectStatusEnum? projectStatus, int page, int size);
    Task<BaseModel<ProjectResponseModel>> UpdateProject(Guid projectId, UpdateProjectRequestModel request);
    Task<BaseModel<ProjectResponseModel>> UpdateProjectStatus(Guid projectId, ProjectStatusEnum newStatus);
    Task<BaseModel<ProjectResponseModel>> GetProjectById(Guid projectId);
    Task<BaseModel<AssignMentorResponseModel>> AssignMentor(Guid projectId, string mentorId);
}