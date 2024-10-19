using System.Security.Claims;
using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.User;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO;
<<<<<<< HEAD
using MBS.DataAccess.Repositories;
=======
>>>>>>> develop
using MBS.DataAccess.Repositories.Interfaces;
using MBS.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class MentorService : BaseService2<MentorService>, IMentorService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ISupabaseService _supabaseService;
    private readonly IConfiguration _configuration;
<<<<<<< HEAD
    private IMentorRepository _mentorRepository;
    public MentorService(
        IUnitOfWork unitOfWork,
        IMentorRepository mentorRepository,
        ILogger<MentorService> logger,
        UserManager<ApplicationUser> userManager,
        ISupabaseService supabaseService, 
        IConfiguration configuration,
        IMapper mapper) : base(unitOfWork, logger, mapper)
=======
    private readonly IMentorRepository _mentorRepository;
    private readonly IDegreeRepository _degreeRepository;

    public MentorService(ILogger<MentorService> logger, IMapper mapper, IMentorRepository mentorRepository,
        UserManager<ApplicationUser> userManager, ISupabaseService supabaseService, IConfiguration configuration,
        IDegreeRepository degreeRepository) : base(logger, mapper)
>>>>>>> develop
    {
        _mentorRepository = mentorRepository;
        _userManager = userManager;
        _supabaseService = supabaseService;
        _configuration = configuration;
<<<<<<< HEAD
        _mentorRepository = mentorRepository;
=======
        _degreeRepository = degreeRepository;
>>>>>>> develop
    }

    public async Task<BaseModel<GetMentorResponseModel>> GetOwnProfile(ClaimsPrincipal claimsPrincipal)
    {
        try
        {
            var userId = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
            {
                return new BaseModel<GetMentorResponseModel>()
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return new BaseModel<GetMentorResponseModel>()
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound
                };
            }

<<<<<<< HEAD
            var mentor = await _mentorRepository.GetMentorbyId(userId);
=======
            var mentor = await _mentorRepository.GetByIdAsync(userId, "UserId");
>>>>>>> develop

            if (mentor is null)
            {
                return new BaseModel<GetMentorResponseModel>()
                {
                    Message = MessageResponseHelper.UserNotFound(),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            }

            return new BaseModel<GetMentorResponseModel>()
            {
                Message = MessageResponseHelper.GetSuccessfully("mentor profile"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = _mapper.Map<GetMentorResponseModel>(mentor)
            };
        }
        catch (Exception e)
        {
            return new BaseModel<GetMentorResponseModel>()
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

            await _degreeRepository.CreateAsync(degree);

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
        ClaimsPrincipal claimsPrincipal, int page, int size)
    {
        try
        {
            var userId = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
            var degrees = await _degreeRepository.GetDegreesByMentorId(userId, page, size);

            return new BaseModel<GetOwnDegreesResponseModel>()
            {
                Message = MessageResponseHelper.GetSuccessfully("degrees"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = new GetOwnDegreesResponseModel()
                {
                    degrees = _mapper.Map<Pagination<GetOwnDegreeResponseModel>>(degrees)
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
            var mentor = await _mentorRepository.GetMentorByIdAsync(request.Id);

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
            var user = await _mentorRepository.GetPagedListAsync(page, size);
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