using System.Security.Claims;
using MBS.Application.Models.General;
using MBS.Application.Models.User;
using MBS.Core.Common.Pagination;

namespace MBS.Application.Services.Interfaces;

public interface IMentorService
{
    Task<BaseModel<GetMentorResponseModel>> GetOwnProfile(ClaimsPrincipal claimsPrincipal);

    Task<BaseModel<UploadOwnDegreeResponseModel, UploadOwnDegreeRequestModel>> UploadOwnDegree(
        UploadOwnDegreeRequestModel request, ClaimsPrincipal claimsPrincipal);

<<<<<<< HEAD
    Task<BaseModel<GetOwnDegreesResponseModel>> GetOwnDegrees(ClaimsPrincipal claimsPrincipal);
=======
    Task<BaseModel<GetOwnDegreesResponseModel>> GetOwnDegrees(ClaimsPrincipal claimsPrincipal, int page, int size);

>>>>>>> parent of 4cb5763 (merge query to test api with data)
    Task<BaseModel<GetMentorResponseModel, GetMentorRequestModel>> GetMentor(GetMentorRequestModel request);
    Task<BaseModel<Pagination<GetMentorResponseModel>>> GetMentors(int page, int size);
}