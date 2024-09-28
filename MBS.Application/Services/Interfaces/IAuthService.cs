using System.Security.Claims;
using MBS.Application.Models.General;
using MBS.Application.Models.User;

namespace MBS.Application.Services.Interfaces;

public interface IAuthService
{
    Task<BaseModel<RegisterResponseModel, RegisterRequestModel>> SignUpAsync(
        RegisterRequestModel request);

    Task<BaseModel<SignInResponseModel, SignInRequestModel>> SignIn(SignInRequestModel request);

    Task<BaseModel<GetRefreshTokenResponseModel, GetRefreshTokenRequestModel>>
        Refresh(GetRefreshTokenRequestModel request);

    Task<BaseModel<ConfirmEmailResponseModel, ConfirmEmailRequestModel>> ConfirmEmailAsync(
        ConfirmEmailRequestModel request);

    Task<BaseModel<ExternalSignInResponseModel, ExternalSignInRequestModel>> LoginOrSignUpExternal(
        ExternalSignInRequestModel request);
}