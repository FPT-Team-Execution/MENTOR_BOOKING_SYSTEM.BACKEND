using MBS.Application.Models.General;
using MBS.Application.Models.Project;
using MBS.Core.Common.Pagination;
using MBS.Core.Enums;

namespace MBS.Application.Services.Interfaces;

public interface IProjectService
{
    Task<BaseModel<Pagination<ProjectResponseDTO>>> GetProjectsByUserId(GetProjectsByUserIdRequest request);
    Task<BaseModel<Pagination<ProjectResponseDTO>>> GetProjectsByStudentId(GetProjectsByStudentIdRequest request);
    Task<BaseModel<ProjectResponseModel>> GetProjectById(Guid projectId);
    Task<BaseModel<CreateProjectResponseModel, CreateProjectRequestModel>> CreateProject(CreateProjectRequestModel request);
    Task<BaseModel<ProjectResponseModel>> UpdateProject(Guid projectId, UpdateProjectRequestModel request);
    // Task<BaseModel<ProjectResponseModel>> UpdateProjectStatus(Guid projectId, ProjectStatusEnum newStatus);
    Task<BaseModel<AssignMentorResponseModel>> AssignMentor(Guid projectId, string mentorId);

    Task<BaseModel<Pagination<ProjectResponseDTO>>> GetAllProjects(int page, int size);

}