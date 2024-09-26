using System.Security.Claims;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.User;
using MBS.Application.Services.Interfaces;
using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Interfaces;
using MBS.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace MBS.Application.Services.Implements;

public class MentorService : IMentorService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMentorRepository _mentorRepository;
    private readonly ISupabaseService _supabaseService;
    private readonly IConfiguration _configuration;

    public MentorService(UserManager<ApplicationUser> userManager, IMentorRepository mentorRepository,
        ISupabaseService supabaseService, IConfiguration configuration)
    {
        _userManager = userManager;
        _mentorRepository = mentorRepository;
        _supabaseService = supabaseService;
        _configuration = configuration;
    }

    public async Task<BaseModel<GetMentorOwnProfileResponseModel>> GetOwnProfile(ClaimsPrincipal claimsPrincipal)
    {
        try
        {
            var userId = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
            {
                return new BaseModel<GetMentorOwnProfileResponseModel>()
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return new BaseModel<GetMentorOwnProfileResponseModel>()
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

            var mentor = await _mentorRepository.GetAsync(x => x.UserId == userId);

            if (mentor is null)
            {
                return new BaseModel<GetMentorOwnProfileResponseModel>()
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }

            return new BaseModel<GetMentorOwnProfileResponseModel>()
            {
                Message = MessageResponseHelper.GetSuccessfully("mentor profile"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = new GetMentorOwnProfileResponseModel()
                {
                    FullName = user.FullName,
                    Birthday = user.Birthday,
                    Gender = user.Gender,
                    AvatarUrl = user.AvatarUrl,
                    Industry = mentor.Industry,
                    ConsumePoint = mentor.ConsumePoint
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<GetMentorOwnProfileResponseModel>()
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }

    public async Task<BaseModel<UploadOwnDegreeResponseModel, UploadOwnDegreeRequestModel>> UploadOwnDegree(
        UploadOwnDegreeRequestModel request, ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;

        var bucketName = _configuration["Supabase:MainBucket"]!;
        var fileByte = await FileHelper.ConvertIFormFileToByteArrayAsync(request.File);
        var fileName = request.File.FileName;
        var filePath = $"Mentors/{userId}/Degrees/{fileName}";
        var degreeUrl = await _supabaseService.UploadFile(fileByte, filePath, bucketName);

        try
        {
            return new BaseModel<UploadOwnDegreeResponseModel, UploadOwnDegreeRequestModel>()
            {
                Message = "",
                IsSuccess = true,
                StatusCode = StatusCodes.Status201Created,
            };
        }
        catch (Exception e)
        {
            return new BaseModel<UploadOwnDegreeResponseModel, UploadOwnDegreeRequestModel>()
            {
                Message = "",
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}