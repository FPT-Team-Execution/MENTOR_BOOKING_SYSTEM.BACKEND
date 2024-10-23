using System.Security.Claims;
using MBS.Application.Models.General;
using MBS.Application.Models.Groups;
using MBS.Application.Models.User;
using MBS.Core.Common.Pagination;

namespace MBS.Application.Services.Interfaces;

public interface IMentorService
{
    Task<BaseModel<GetMentorResponseModel>> GetOwnProfile(ClaimsPrincipal claimsPrincipal);

    Task<BaseModel<UploadOwnDegreeResponseModel, UploadOwnDegreeRequestModel>> UploadOwnDegree(
        UploadOwnDegreeRequestModel request, ClaimsPrincipal claimsPrincipal);

    Task<BaseModel<List<MentorSearchDTO>>> SearchMentor(string searchItem);
    //Task<BaseModel<GetOwnDegreesResponseModel>> GetOwnDegrees(ClaimsPrincipal claimsPrincipal);

    Task<BaseModel<GetOwnDegreesResponseModel>> GetOwnDegrees(ClaimsPrincipal claimsPrincipal, int page, int size);

    Task<BaseModel<GetMentorResponseModel, GetMentorRequestModel>> GetMentor(GetMentorRequestModel request);
    Task<BaseModel<Pagination<GetMentorResponseModel>>> GetMentors(int page, int size);
}