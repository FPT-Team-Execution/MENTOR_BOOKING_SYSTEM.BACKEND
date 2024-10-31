using System.Drawing;
using System.Security.Claims;
using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Groups;
using MBS.Application.Models.Mentor;
using MBS.Application.Models.User;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
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
	private readonly IMentorRepository _mentorRepository;
	private readonly IDegreeRepository _degreeRepository;

	public MentorService(ILogger<MentorService> logger, IMapper mapper, IMentorRepository mentorRepository,
		UserManager<ApplicationUser> userManager, ISupabaseService supabaseService, IConfiguration configuration,
		IDegreeRepository degreeRepository) : base(logger, mapper)
	{
		_mentorRepository = mentorRepository;
		_userManager = userManager;
		_supabaseService = supabaseService;
		_configuration = configuration;
		_degreeRepository = degreeRepository;
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

			var mentor = await _mentorRepository.GetByIdAsync(userId, "UserId");

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

	public async Task<BaseModel<UpdateMentorResponseModel>> UpdateOwnProfile(ClaimsPrincipal User, UpdateMentorRequestModel request)
	{
		try
		{
			var mentor = await _mentorRepository.GetMentorByIdAsync(request.Id);
			if (mentor is null)
			{
				return new BaseModel<UpdateMentorResponseModel>()
				{
					Message = MessageResponseHelper.UserNotFound(),
					IsSuccess = true,
					StatusCode = StatusCodes.Status200OK
				};
			}
			mentor.Industry = request.Industry;
			mentor.ConsumePoint = request.ConsumePoint;

			var user = mentor.User;
			user.FullName = request.FullName;
			user.Birthday = request.Birthday;
			user.Gender = request.Gender;
			user.PhoneNumber = request.PhoneNumber;
			user.LockoutEnd = request.LockoutEnd;
			user.LockoutEnabled = request.LockoutEnabled;
			user.UpdatedBy = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value.ToString();
			user.UpdatedOn = DateTime.UtcNow;

			_mentorRepository.Update(mentor);
			_userManager.UpdateAsync(user);

			return new BaseModel<UpdateMentorResponseModel>()
			{
				Message = MessageResponseHelper.Successfully("Update mentor"),
				IsSuccess = true,
				StatusCode = StatusCodes.Status200OK
			};
		}
		catch (Exception e)
		{
			return new BaseModel<UpdateMentorResponseModel>()
			{
				Message = e.Message,
				IsSuccess = false,
				StatusCode = StatusCodes.Status500InternalServerError,
				ResponseRequestModel = new UpdateMentorResponseModel()
				{
					Succeed = false
				}
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


	public async Task<BaseModel<List<MentorSearchDTO>>> SearchMentor(string searchItem)
	{
		var searching = searchItem.ToLower();
		var mentorSearch = await _mentorRepository.GetMentorsAsync();
		List<MentorSearchDTO> mentorSearchDTOs = new List<MentorSearchDTO>();
		if (mentorSearch != null && mentorSearch.Any())
		{
			foreach (var item in mentorSearch)
			{
				var searchMentor = await _mentorRepository.GetByUserIdAsync(item.UserId, m => m.Include(x => x.User));

				string searchByName = searchMentor.User.FullName.ToLower();
				string searchByEmail = searchMentor.User.Email.ToLower();
				if (searchByName.Contains(searchItem) || searchByEmail.Contains(searchItem))
				{
					mentorSearchDTOs.Add(new MentorSearchDTO
					{
						MentorId = item.UserId,
						FullName = searchMentor.User.FullName,
						Email = searchMentor.User.Email,
					});
				}
			}
		}

		var response = mentorSearchDTOs;
		return new BaseModel<List<MentorSearchDTO>>()
		{
			Message = MessageResponseHelper.GetSuccessfully("mentors"),
			IsSuccess = true,
			StatusCode = StatusCodes.Status200OK,
			ResponseRequestModel = response
		};
	}

	public async Task<BaseModel<GetMentorResponseModel, GetMentorRequestModel>> GetMentor(
		GetMentorRequestModel request)
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
			var user = await _mentorRepository.GetMentorsAsync(page, size);
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

	public async Task<BaseModel<Pagination<GetMentorDegreeResponseModel>>> GetMentorDegrees(GetMentorDegreesRequestModel request)
	{
		try
		{
			var user = await _mentorRepository.GetMentorByIdAsync(request.MentorId);
			if (user == null)
			{
				return new BaseModel<Pagination<GetMentorDegreeResponseModel>>()
				{
					IsSuccess = false,
					Message = MessageResponseHelper.UserNotFound(),
					ResponseRequestModel = null,
					StatusCode = StatusCodes.Status404NotFound
				};
			}
			var degrees = await _degreeRepository.GetDegreesByMentorId(request.MentorId, request.Page, request.Size);

			return new BaseModel<Pagination<GetMentorDegreeResponseModel>>()
			{
				Message = MessageResponseHelper.GetSuccessfully("degrees"),
				IsSuccess = true,
				StatusCode = StatusCodes.Status200OK,
				ResponseRequestModel = _mapper.Map<Pagination<GetMentorDegreeResponseModel>>(degrees)
			};
		}
		catch (Exception e)
		{
			return new BaseModel<Pagination<GetMentorDegreeResponseModel>>()
			{
				Message = e.Message,
				IsSuccess = false,
				StatusCode = StatusCodes.Status500InternalServerError,
			};
		}
	}

	public async Task<BaseModel<UpdateMentorResponseModel>> UpdateMentorProfile(UpdateMentorRequestModel request)
	{
		try
		{
			var mentor = await _mentorRepository.GetMentorByIdAsync(request.Id);
			if (mentor is null)
			{
				return new BaseModel<UpdateMentorResponseModel>()
				{
					Message = MessageResponseHelper.UserNotFound(),
					IsSuccess = true,
					StatusCode = StatusCodes.Status200OK
				};
			}
			mentor.Industry = request.Industry;
			mentor.ConsumePoint = request.ConsumePoint;

			var user = mentor.User;
			user.FullName = request.FullName;
			user.Birthday = request.Birthday;
			user.Gender = request.Gender;
			user.PhoneNumber = request.PhoneNumber;
			user.LockoutEnd = request.LockoutEnd;
			user.LockoutEnabled = request.LockoutEnabled;
			user.UpdatedBy = "Admin";
			user.UpdatedOn = DateTime.UtcNow;

			_mentorRepository.Update(mentor);
			_userManager.UpdateAsync(user);

			return new BaseModel<UpdateMentorResponseModel>()
			{
				Message = MessageResponseHelper.Successfully("Update mentor"),
				IsSuccess = true,
				StatusCode = StatusCodes.Status200OK
			};
		}
		catch (Exception e)
		{
			return new BaseModel<UpdateMentorResponseModel>()
			{
				Message = e.Message,
				IsSuccess = false,
				StatusCode = StatusCodes.Status500InternalServerError,
				ResponseRequestModel = new UpdateMentorResponseModel()
				{
					Succeed = false
				}
			};
		}
	}
}