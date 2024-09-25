using System.Security.Claims;
using MBS.Application.Common.Email;
using MBS.Application.Exceptions;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.User;
using MBS.Application.Services.Interfaces;
using MBS.Application.Templates;
using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace MBS.Application.Services.Implements;

public class AuthService : IAuthService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMentorRepository _mentorRepository;
    private readonly IEmailService _emailService;
    private readonly ITemplateService _templateService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthService
    (
        IStudentRepository studentRepository,
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager, IMentorRepository mentorRepository, IConfiguration configuration,
        IEmailService emailService, ITemplateService templateService)
    {
        _studentRepository = studentRepository;
        _roleManager = roleManager;
        _userManager = userManager;
        _mentorRepository = mentorRepository;
        _configuration = configuration;
        _emailService = emailService;
        _templateService = templateService;
    }

    public async Task<BaseModel<RegisterStudentResponseModel, RegisterStudentRequestModel>> SignUpStudentAsync(
        RegisterStudentRequestModel request)
    {
        try
        {
            var existUser = await _userManager.FindByEmailAsync(request.Email);

            if (existUser is not null)
            {
                return new()
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

            var addStudentResult = await _studentRepository.AddAsync(newStudent);

            if (addStudentResult is false)
            {
                throw new DatabaseInsertException("student");
            }

            await SendVerifyEmail(newUser);

            return new BaseModel<RegisterStudentResponseModel, RegisterStudentRequestModel>()
            {
                Message = MessageResponseHelper.Register("student"),
                StatusCode = StatusCodes.Status201Created,
                IsSuccess = true,
                ResponseModel = new RegisterStudentResponseModel()
                {
                    UserId = newUser.Id
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<RegisterStudentResponseModel, RegisterStudentRequestModel>()
            {
                Message = e.Message,
                StatusCode = StatusCodes.Status500InternalServerError,
                IsSuccess = false,
                RequestModel = request
            };
        }
    }

    public async Task<BaseModel<RegisterMentorResponseModel, RegisterMentorRequestModel>> SignUpMentorAsync(
        RegisterMentorRequestModel request)
    {
        try
        {
            var existUser = await _userManager.FindByEmailAsync(request.Email);

            if (existUser is not null)
            {
                return new()
                {
                    Message = MessageResponseHelper.UserWasExisted(request.Email),
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false,
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

            var assignRoleResult = await _userManager.AddToRoleAsync(newUser, UserRoleEnum.Mentor.ToString());

            if (!assignRoleResult.Succeeded)
            {
                throw new DatabaseInsertException("role");
            }

            var newMentor = new Mentor()
            {
                UserId = newUser.Id,
                Industry = request.Industry
            };

            var addMentorResult = await _mentorRepository.AddAsync(newMentor);

            if (addMentorResult is false)
            {
                throw new DatabaseInsertException("mentor");
            }

            await SendVerifyEmail(newUser);

            return new BaseModel<RegisterMentorResponseModel, RegisterMentorRequestModel>()
            {
                Message = MessageResponseHelper.Register("mentor"),
                StatusCode = StatusCodes.Status201Created,
                IsSuccess = true,
                ResponseModel = new RegisterMentorResponseModel()
                {
                    UserId = newUser.Id
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<RegisterMentorResponseModel, RegisterMentorRequestModel>()
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
}