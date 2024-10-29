using System.Security.Claims;
using MBS.Application.Models.General;
using MBS.Application.Models.Student;
using MBS.Application.Models.User;
using MBS.Core.Common.Pagination;

namespace MBS.Application.Services.Interfaces;

public interface IStudentService
{
    Task<BaseModel<Pagination<StudentResponseDto>>> GetStudents(int page, int size, string? sortOrder);
    Task<BaseModel<UpdateStudentResponseModel>> UpdateOwnProfile(ClaimsPrincipal User, UpdateStudentRequestModel request);

    Task<BaseModel<GetStudentResponseModel, GetStudentRequestModel>> GetOwnProfile(
        ClaimsPrincipal claimsPrincipal);

    Task<BaseModel<GetStudentResponseModel, GetStudentRequestModel>> GetStudent(GetStudentRequestModel request);
}