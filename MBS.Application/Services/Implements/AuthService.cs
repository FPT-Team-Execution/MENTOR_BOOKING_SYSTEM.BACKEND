using System.Security.Claims;
using AutoMapper;
using MBS.Application.Exceptions;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.User;
using MBS.Application.Services.Interfaces;
using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess.DAO;
using MBS.DataAccess.Repositories.Interfaces;
using MBS.Shared.Common.Email;
using MBS.Shared.Services.Interfaces;
using MBS.Shared.Templates;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class AuthService : BaseService2<AuthService>, IAuthService
{
    private readonly IMentorRepository _mentorRepository;
    private readonly ITemplateService _templateService;
    private readonly IEmailService _emailService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IStudentRepository _studentRepository;
    private readonly IConfiguration _configuration;
    private readonly ISupabaseService _supabaseService;

    public AuthService(ILogger<AuthService> logger,
        IStudentRepository studentRepository,
        IMentorRepository mentorRepository,
        IEmailService emailService,
        ITemplateService templateService,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        IMapper mapper, ISupabaseService supabaseService)
        : base(logger, mapper)
    {
        _studentRepository = studentRepository;
        _mentorRepository = mentorRepository;
        _emailService = emailService;
        _templateService = templateService;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _supabaseService = supabaseService;
    }

    public async Task<BaseModel<RegisterResponseModel, RegisterRequestModel>> SignUpAsync(
        RegisterRequestModel request)
    {
        try
        {
            switch (request.Role.Trim().ToUpper())
            {
                case var role when role == nameof(UserRoleEnum.Student).ToUpper():
                {
                    var existUser = await _userManager.FindByEmailAsync(request.Email);

                    if (existUser is not null)
                    {
                        return new BaseModel<RegisterResponseModel, RegisterRequestModel>
                        {
                            Message = MessageResponseHelper.UserWasExisted(request.Email),
                            StatusCode = StatusCodes.Status400BadRequest,
                            IsSuccess = false
                        };
                    }

                    var newUser = new ApplicationUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = request.Email,
                        UserName = request.Email,
                        Gender = request.Gender,
                        FullName = request.FullName,
                        CreatedBy = request.Email,
                        CreatedOn = DateTime.UtcNow,
                    };

                    var createUserResult = await _userManager.CreateAsync(newUser, request.Password);

                    if (!createUserResult.Succeeded)
                    {
                        throw new DatabaseInsertException("user");
                    }

                    var assignRoleResult = await _userManager.AddToRoleAsync(newUser, UserRoleEnum.Student.ToString());

                    if (!assignRoleResult.Succeeded)
                    {
                        throw new DatabaseInsertException("role");
                    }

                    var newStudent = new Student()
                    {
                        UserId = newUser.Id,
                        University = request.University,
                        MajorId = request.MajorId,
                        WalletPoint = 0
                    };

                    var result = await _studentRepository.CreateAsync(newStudent);

                    if (!result)

                        // await _unitOfWork.GetRepository<Student>().InsertAsync(newStudent);
                        //
                        // if (await _unitOfWork.CommitAsync() <= 0)

                    {
                        throw new DatabaseInsertException("student");
                    }

                    await SendVerifyEmail(newUser);

                    return new BaseModel<RegisterResponseModel, RegisterRequestModel>()
                    {
                        Message = MessageResponseHelper.Register("student"),
                        StatusCode = StatusCodes.Status201Created,
                        IsSuccess = true,
                        ResponseModel = new RegisterResponseModel()
                        {
                            UserId = newUser.Id
                        }
                    };
                }
                case var role when role == nameof(UserRoleEnum.Admin).ToUpper():
                {
                    return new BaseModel<RegisterResponseModel, RegisterRequestModel>()
                    {
                        Message = MessageResponseHelper.Invalid("role"),
                        IsSuccess = false,
                        RequestModel = request,
                        StatusCode = StatusCodes.Status406NotAcceptable
                    };
                }
                default:
                {
                    return new BaseModel<RegisterResponseModel, RegisterRequestModel>()
                    {
                        Message = MessageResponseHelper.Invalid("role"),
                        IsSuccess = false,
                        RequestModel = request,
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
            }
        }
        catch (Exception e)
        {
            return new BaseModel<RegisterResponseModel, RegisterRequestModel>()
            {
                Message = e.Message,
                StatusCode = StatusCodes.Status500InternalServerError,
                IsSuccess = false,
                RequestModel = request
            };
        }
    }

    public async Task<BaseModel<SignInResponseModel, SignInRequestModel>> SignIn(SignInRequestModel request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new BaseModel<SignInResponseModel, SignInRequestModel>()
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    RequestModel = request,
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordCorrect)
            {
                return new BaseModel<SignInResponseModel, SignInRequestModel>()
                {
                    Message = MessageResponseHelper.IncorrectEmailOrPassword(),
                    IsSuccess = false,
                    RequestModel = request,
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }

            if (!user.EmailConfirmed)
            {
                await SendVerifyEmail(user);
                return new BaseModel<SignInResponseModel, SignInRequestModel>()
                {
                    Message = MessageResponseHelper.EmailNotConfirmed(),
                    IsSuccess = false,
                    RequestModel = request,
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }

            if (user.LockoutEnd is not null)
            {
                return new BaseModel<SignInResponseModel, SignInRequestModel>()
                {
                    Message = MessageResponseHelper.UserLocked(),
                    IsSuccess = false,
                    RequestModel = request,
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }

            var accessToken = JwtHelper.GenerateJwtAccessTokenAsync(user, _userManager, _configuration);
            var refreshToken = JwtHelper.GenerateJwtRefreshTokenAsync(user, _configuration);

            return new BaseModel<SignInResponseModel, SignInRequestModel>()
            {
                Message = MessageResponseHelper.Login(),
                IsSuccess = true,
                ResponseModel = new SignInResponseModel()
                {
                    JwtToken = new JwtModel()
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    }
                },
                StatusCode = StatusCodes.Status200OK
            };
        }
        catch (Exception e)
        {
            return new BaseModel<SignInResponseModel, SignInRequestModel>()
            {
                Message = e.Message,
                StatusCode = StatusCodes.Status500InternalServerError,
                IsSuccess = false,
                RequestModel = request
            };
        }
    }

    public async Task<BaseModel<GetRefreshTokenResponseModel, GetRefreshTokenRequestModel>> Refresh(
        GetRefreshTokenRequestModel request)
    {
        try
        {
            ClaimsPrincipal? claimsPrincipal = JwtHelper.GetPrincipalFromJwtToken(request.RefreshToken, _configuration);

            if (claimsPrincipal is null)
            {
                return new BaseModel<GetRefreshTokenResponseModel, GetRefreshTokenRequestModel>()
                {
                    Message = MessageResponseHelper.TokenInvalid(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    RequestModel = request
                };
            }

            var userId = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return new BaseModel<GetRefreshTokenResponseModel, GetRefreshTokenRequestModel>()
                {
                    Message = MessageResponseHelper.TokenInvalid(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    RequestModel = request
                };
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return new BaseModel<GetRefreshTokenResponseModel, GetRefreshTokenRequestModel>()
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    RequestModel = request
                };
            }

            var jwtModel = new JwtModel()
            {
                AccessToken = JwtHelper.GenerateJwtAccessTokenAsync(user, _userManager, _configuration),
                RefreshToken = JwtHelper.GenerateJwtRefreshTokenAsync(user, _configuration)
            };

            return new BaseModel<GetRefreshTokenResponseModel, GetRefreshTokenRequestModel>()
            {
                Message = MessageResponseHelper.RefreshTokenSuccessfully(),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseModel = new GetRefreshTokenResponseModel()
                {
                    NewJwtToken = jwtModel
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<GetRefreshTokenResponseModel, GetRefreshTokenRequestModel>()
            {
                Message = e.Message,
                StatusCode = StatusCodes.Status500InternalServerError,
                IsSuccess = false,
                RequestModel = request
            };
        }
    }

    public async Task<BaseModel<ConfirmEmailResponseModel, ConfirmEmailRequestModel>> ConfirmEmailAsync(
        ConfirmEmailRequestModel request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return new BaseModel<ConfirmEmailResponseModel, ConfirmEmailRequestModel>()
                {
                    Message = MessageResponseHelper.UserNotFound(request.Email),
                    StatusCode = StatusCodes.Status404NotFound,
                    IsSuccess = false,
                    RequestModel = request
                };
            }

            var confirmResult = await _userManager.ConfirmEmailAsync(user, request.Token);

            if (!confirmResult.Succeeded)
            {
                return new BaseModel<ConfirmEmailResponseModel, ConfirmEmailRequestModel>()
                {
                    Message = MessageResponseHelper.ConfirmEmailFailed(request.Email),
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false,
                    ResponseModel = new ConfirmEmailResponseModel()
                    {
                        Confirmed = false
                    },
                    RequestModel = request
                };
            }

            return new BaseModel<ConfirmEmailResponseModel, ConfirmEmailRequestModel>()
            {
                Message = MessageResponseHelper.ConfirmEmailSucceeded(request.Email),
                IsSuccess = true,
                ResponseModel = new ConfirmEmailResponseModel()
                {
                    Confirmed = true
                },
                StatusCode = StatusCodes.Status200OK
            };
        }
        catch (Exception e)
        {
            return new BaseModel<ConfirmEmailResponseModel, ConfirmEmailRequestModel>()
            {
                Message = e.Message,
                StatusCode = StatusCodes.Status500InternalServerError,
                IsSuccess = false,
                RequestModel = request,
                ResponseModel = new ConfirmEmailResponseModel()
                {
                    Confirmed = false
                }
            };
        }
    }

    private async Task SendVerifyEmail(ApplicationUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var emailTemplate = await _templateService.GetTemplateAsync(TemplateConstants.ConfirmationEmail);

        var emailBody = _templateService.ReplaceInTemplate(emailTemplate,
            new Dictionary<string, string> { { "{Email}", user.Email! }, { "{Token}", token } });

        await _emailService.SendEmailAsync(EmailMessage.Create(user.Email!, emailBody, "[MBS]Confirm your email"));
    }

    public async Task<BaseModel<ExternalSignInResponseModel>> LoginOrSignUpExternal(
        ExternalSignInRequestModel request)
    {
        // var provider = request.authenticationResult;
        var profile = request.profile;
        var token = request.token;
        //Try Sign in by external information
        var tryExternalLogin =
            await _signInManager.ExternalLoginSignInAsync("Google", profile.sub, true);
        //if success, get info user and return result
        if (tryExternalLogin.Succeeded)
        {
            var user = await _userManager.FindByLoginAsync("Google", profile.sub);
            if (user == null)
            {
                return new BaseModel<ExternalSignInResponseModel>
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    StatusCode = StatusCodes.Status404NotFound,
                    IsSuccess = false,
                };
            }

            //return response
            var accessToken = JwtHelper.GenerateJwtAccessTokenAsync(user, _userManager, _configuration);
            var refreshToken = JwtHelper.GenerateJwtRefreshTokenAsync(user, _configuration);
            return new BaseModel<ExternalSignInResponseModel>
            {
                Message = MessageResponseHelper.GetSuccessfully("athorization"),
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = true,
                ResponseRequestModel = new ExternalSignInResponseModel
                {
                    JwtModel = new JwtModel
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                    },
                    //TODO: return refresh token
                    GoogleToken = token
                }
            };
        }

        //if user is new -> create new account
        var userCreate = new ApplicationUser
        {
            Email = profile.email,
            UserName = profile.email,
            FullName = profile.name,
            AvatarUrl = profile.picture,
            EmailConfirmed = profile.email_verified
            //TODO: get more info from email
        };
        //create user, add role,add external login 
        //* create user
        var createResult = await _userManager.CreateAsync(userCreate);
        if (createResult.Succeeded)
        {
            //create mentor
            var mentorCreate = new Mentor()
            {
                UserId = userCreate.Id
            };
            var addMentorResult = await _mentorRepository.CreateAsync(mentorCreate);
            if (!addMentorResult)
            {
                return new BaseModel<ExternalSignInResponseModel>
                {
                    Message = MessageResponseHelper.CreateFailed("mentor"),
                    StatusCode = StatusCodes.Status500InternalServerError,
                    IsSuccess = false
                };
            }

            //*add role
            var user = await _userManager.FindByEmailAsync(userCreate.Email);
            await _userManager.AddToRoleAsync(user, UserRoleEnum.Mentor.ToString());
            //*Add external login
            var userLoginInfo =
                new UserLoginInfo(providerKey: profile.sub, loginProvider: "Google", displayName: "Google");
            var addResult = await _userManager.AddLoginAsync(userCreate, userLoginInfo);
            if (addResult.Succeeded)
            {
                var accessToken = JwtHelper.GenerateJwtAccessTokenAsync(userCreate, _userManager, _configuration);
                var refreshToken = JwtHelper.GenerateJwtRefreshTokenAsync(userCreate, _configuration);
                return new BaseModel<ExternalSignInResponseModel>
                {
                    Message = MessageResponseHelper.GetSuccessfully("authentication"),
                    StatusCode = StatusCodes.Status200OK,
                    IsSuccess = true,
                    ResponseRequestModel = new ExternalSignInResponseModel
                    {
                        JwtModel = new JwtModel
                        {
                            AccessToken = accessToken,
                            RefreshToken = refreshToken,
                        },
                        //TODO: return refresh token
                        GoogleToken = token
                    }
                };
            }
        }

        return new BaseModel<ExternalSignInResponseModel>
        {
            Message = "",
            StatusCode = StatusCodes.Status500InternalServerError,
            IsSuccess = false,
        };
    }

    public async Task<BaseModel<UploadAvatarResponseModel, UploadAvatarRequestModel>> UploadAvatar(
        UploadAvatarRequestModel request, ClaimsPrincipal claimsPrincipal)
    {
        try
        {
            var userId = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return new BaseModel<UploadAvatarResponseModel, UploadAvatarRequestModel>()
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    RequestModel = request,
                    StatusCode = StatusCodes.Status404NotFound,
                    ResponseModel = null
                };
            }

            var bucketName = _configuration["Supabase:MainBucket"]!;
            var fileByte = await FileHelper.ConvertIFormFileToByteArrayAsync(request.File);
            var fileName = request.File.FileName;
            var filePath = $"Users/{userId}/Avatar/{fileName}";

            await _supabaseService.UploadFile(fileByte, filePath, bucketName);

            var avatarUrl = _supabaseService.RetrievePublicUrl(bucketName, filePath);

            user.AvatarUrl = avatarUrl;

            await _userManager.UpdateAsync(user);

            return new BaseModel<UploadAvatarResponseModel, UploadAvatarRequestModel>()
            {
                Message = MessageResponseHelper.Successfully("Upload avatar"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status201Created,
                ResponseModel = new UploadAvatarResponseModel()
                {
                    AvatarUrl = avatarUrl
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<UploadAvatarResponseModel, UploadAvatarRequestModel>()
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}