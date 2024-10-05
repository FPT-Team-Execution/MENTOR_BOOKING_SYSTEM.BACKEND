using System.Security.Claims;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.User;
using MBS.Application.Services.Interfaces;
using MBS.Core.Entities;
using MBS.DataAccess.Repositories;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class StudentService : BaseService<StudentService>, IStudentService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public StudentService(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, ILogger<StudentService> logger) : base(unitOfWork, logger)
    {
        _userManager = userManager;
    }

    public async Task<BaseModel<GetStudentOwnProfileResponseModel>> GetOwnProfile(
        ClaimsPrincipal claimsPrincipal)
    {
        try
        {
            var userId = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return new BaseModel<GetStudentOwnProfileResponseModel>()
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }

            var student = await _unitOfWork.GetRepository<Student>().SingleOrDefaultAsync(x => x.UserId == userId);

            if (student is null)
            {
                return new BaseModel<GetStudentOwnProfileResponseModel>()
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }

            return new BaseModel<GetStudentOwnProfileResponseModel>()
            {
                Message = MessageResponseHelper.GetSuccessfully("student profile"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = new GetStudentOwnProfileResponseModel()
                {
                    Gender = user.Gender,
                    FullName = user.FullName,
                    Birthday = user.Birthday,
                    AvatarUrl = user.AvatarUrl,
                    University = student.University,
                    WalletPoint = student.WalletPoint
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<GetStudentOwnProfileResponseModel>()
            {
                Message = e.Message,
                StatusCode = StatusCodes.Status500InternalServerError,
                IsSuccess = false,
            };
        }
    }
}