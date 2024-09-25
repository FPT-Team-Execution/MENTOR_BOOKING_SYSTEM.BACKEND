using System.Security.Claims;
using MBS.Application.Models.General;
using MBS.Application.Models.User;

namespace MBS.Application.Services.Interfaces;

public interface IUserService
{
    Task<BaseModel<RegisterStudentResponseModel, RegisterStudentRequestModel>> SignUpStudentAsync(
        RegisterStudentRequestModel request);

    Task<BaseModel<RegisterMentorResponseModel, RegisterMentorRequestModel>> SignUpMentorAsync(
        RegisterMentorRequestModel request);

    Task<BaseModel<SignInResponseModel, SignInRequestModel>> SignIn(SignInRequestModel request);

    Task<BaseModel<GetRefreshTokenResponseModel, GetRefreshTokenRequestModel>>
        Refresh(GetRefreshTokenRequestModel request);

    Task<BaseModel<ConfirmEmailResponseModel, ConfirmEmailRequestModel>> ConfirmEmailAsync(
        ConfirmEmailRequestModel request);

    Task<BaseModel<GetStudentOwnProfileResponseModel>> GetStudentOwnProfile(
        ClaimsPrincipal claimsPrincipal);

    Task<BaseModel<GetMentorOwnProfileResponseModel>> GetMentorOwnProfile(ClaimsPrincipal claimsPrincipal);
}