using MBS.Application.Models.General;
using MBS.Application.Models.Project;
using MBS.Core.Enums;

namespace MBS.Application.Services.Interfaces;

public interface IProjectService
{
    Task<BaseModel<CreateProjectResponseModel, CreateProjectRequestModel>> CreateProject(CreateProjectRequestModel request);

    Task<BaseModel<GetProjectsByStudentIdResponseModel>> GetProjectsByStudentId(string studentId, ProjectStatusEnum? projectStatus, int page, int size);
    Task<BaseModel<UpdateProjectResponseModel>> UpdateProject(Guid projectId, UpdateProjectRequestModel request);
    Task<BaseModel<UpdateProjectStatusResponseModel>> UpdateProjectStatus(Guid projectId, ProjectStatusEnum newStatus);
    Task<BaseModel<GetProjectByIdResponseModel>> GetProjectById(Guid projectId);

    Task<BaseModel<AssignMentorResponseModel>> AssignMentor(Guid projectId, string mentorId);
}