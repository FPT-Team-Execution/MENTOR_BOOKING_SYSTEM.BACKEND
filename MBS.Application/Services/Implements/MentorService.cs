using System.Security.Claims;
using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.User;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO;
using MBS.DataAccess.Repositories;
using MBS.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class MentorService : BaseService<MentorService>, IMentorService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ISupabaseService _supabaseService;
    private readonly IConfiguration _configuration;

    public MentorService(
        IUnitOfWork unitOfWork,
        ILogger<MentorService> logger,
        UserManager<ApplicationUser> userManager,
        ISupabaseService supabaseService, 
        IConfiguration configuration,
        IMapper mapper) : base(unitOfWork, logger, mapper)
    {
        _userManager = userManager;
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

            var mentor = await _unitOfWork.GetRepository<Mentor>().SingleOrDefaultAsync(x => x.UserId == userId);

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
        try
        {
            var userId = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
            var bucketName = _configuration["Supabase:MainBucket"]!;
            var fileByte = await FileHelper.ConvertIFormFileToByteArrayAsync(request.File);
            var fileName = request.File.FileName;
            var filePath = $"Mentors/{userId}/Degrees/{fileName}";
            await _supabaseService.UploadFile(fileByte, filePath, bucketName);
            var degreeUrl = _supabaseService.RetrievePublicUrl(bucketName, filePath);

            var degree = new Degree()
            {
                Id = Guid.NewGuid(),
                Institution = request.Institution,
                Name = request.Name,
                MentorId = userId,
                ImageUrl = degreeUrl
            };

            await _unitOfWork.GetRepository<Degree>().InsertAsync(degree);

            return new BaseModel<UploadOwnDegreeResponseModel, UploadOwnDegreeRequestModel>()
            {
                Message = MessageResponseHelper.UploadSuccessfully("degree"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status201Created,
                ResponseModel = new UploadOwnDegreeResponseModel()
                {
                    DegreeId = degree.Id,
                    DegreeName = degree.Name,
                    DegreeUrl = degree.ImageUrl
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<UploadOwnDegreeResponseModel, UploadOwnDegreeRequestModel>()
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<GetOwnDegreesResponseModel>> GetOwnDegrees(
        ClaimsPrincipal claimsPrincipal)
    {
        try
        {
            var userId = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
            var degrees = await _unitOfWork.GetRepository<Degree>().GetListAsync(x => x.MentorId == userId);

            var degreeResponseModels = degrees.Select(x => new GetOwnDegreeResponseModel()
            {
                Id = x.Id,
                Name = x.Name,
                Institution = x.Institution,
                ImageUrl = x.ImageUrl
            });

            return new BaseModel<GetOwnDegreesResponseModel>()
            {
                Message = MessageResponseHelper.GetSuccessfully("degrees"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = new GetOwnDegreesResponseModel()
                {
                    DegreeResponseModels = degreeResponseModels
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<GetOwnDegreesResponseModel>()
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<GetMentorResponseModel, GetMentorRequestModel>> GetMentor(GetMentorRequestModel request)
    {
        try
        {
            var mentor = await _unitOfWork.GetRepository<Mentor>().SingleOrDefaultAsync
            (
                predicate: x => x.UserId == request.Id,
                include: x => x.Include(x => x.User)
            );

            if (mentor is null)
            {
                return new BaseModel<GetMentorResponseModel, GetMentorRequestModel>()
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    RequestModel = request,
                    ResponseModel = null
                };
            }

            var response = _mapper.Map<GetMentorResponseModel>(mentor);

            return new BaseModel<GetMentorResponseModel, GetMentorRequestModel>()
            {
                Message = MessageResponseHelper.GetSuccessfully("mentor"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseModel = response,
                RequestModel = request
            };
        }
        catch (Exception e)
        {
            return new BaseModel<GetMentorResponseModel, GetMentorRequestModel>()
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
                RequestModel = request,
                ResponseModel = null
            };
        }
    }

    public async Task<BaseModel<Pagination<GetMentorResponseModel>>> GetMentors(int page, int size)
    {
        try
        {
            var user = await _unitOfWork.GetRepository<Mentor>().GetPagingListAsync(
                include: s => s.Include(x => x.User),
                page: page,
                size: size
            );
            return new BaseModel<Pagination<GetMentorResponseModel>>()
            {
                Message = MessageResponseHelper.GetSuccessfully("students"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = _mapper.Map<Pagination<GetMentorResponseModel>>(user)
            };
        }
        catch (Exception e)
        {
            return new BaseModel<Pagination<GetMentorResponseModel>>()
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}