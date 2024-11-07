using System.Linq.Expressions;
using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.Feedback;
using MBS.Application.Models.General;
using MBS.Application.Models.Meeting;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class FeedbackService : BaseService2<FeedbackService>, IFeedbackService
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMeetingRepository _meetingRepository;
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IMentorRepository _mentorRepository;


    public FeedbackService(
        IFeedbackRepository feedbackRepository,
        IMeetingRepository meetingRepository,
        IMentorRepository mentorRepository,
        UserManager<ApplicationUser> userManager,
        ILogger<FeedbackService> logger,
        IMapper mapper) : base(logger, mapper)
    {
        _feedbackRepository = feedbackRepository;
        _meetingRepository = meetingRepository;
        _userManager = userManager;
        _mentorRepository = mentorRepository;
    }

    

    public async Task<BaseModel<Pagination<FeedbackResponseDTO>>> GetMeetingFeedbacksByUserId(GetMeetingFeedbacksByUserIdRequest request)
    {
        try
        {
            //check meeting
            var meeting = await _meetingRepository.GetByIdAsync(request.MeetingId, "Id");
            if (meeting == null)
                return new BaseModel<Pagination<FeedbackResponseDTO>>
                {
                    Message = MessageResponseHelper.DetailException("meeting", request.MeetingId.ToString(), "not found", "Id"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return new BaseModel<Pagination<FeedbackResponseDTO>>
                {
                    Message = MessageResponseHelper.DetailException("student", request.UserId, "not found", "Id"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            //get all
            var feedbacks =
                await _feedbackRepository.GetMeetingFeedBacksByUserId(request.MeetingId, request.UserId, request.Page, request.Size, request.SortOrder);
            return new BaseModel<Pagination<FeedbackResponseDTO>>
            {
                Message = MessageResponseHelper.GetSuccessfully("feedbacks"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = _mapper.Map<Pagination<FeedbackResponseDTO>>(feedbacks)
			};
        }
        catch (Exception e)
        {
            return new BaseModel<Pagination<FeedbackResponseDTO>>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<Pagination<FeedbackResponseDTO>>> GetFeedbacksByMeetingId(GetFeedbacksByMeetingIdRequest request)
    {
        try {
            //check meeting
            var meeting = await _meetingRepository.GetByIdAsync(request.MeetingId, "Id");
            if (meeting == null)
                return new BaseModel<Pagination<FeedbackResponseDTO>>
                {
                    Message = MessageResponseHelper.DetailException("meeting", request.MeetingId.ToString(), "not found", "Id"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            //get all
            var feedbacks =
                await _feedbackRepository.GetFeedBacksByMeetingId(request.MeetingId, request.Page, request.Size, request.SortOrder);
            return new BaseModel<Pagination<FeedbackResponseDTO>>
            {
                Message = MessageResponseHelper.GetSuccessfully("feedbacks"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = _mapper.Map<Pagination<FeedbackResponseDTO>>(feedbacks)
			};
        }
        catch (Exception e)
        {
            return new BaseModel<Pagination<FeedbackResponseDTO>>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<FeedbackModel>> GetFeedbackById(Guid feedbackId)
    {
        try {
            var feedback = await _feedbackRepository.GetByIdAsync(feedbackId, "Id");
            if (feedback == null)
                return new BaseModel<FeedbackModel>
                {
                    Message = MessageResponseHelper.DetailException("feedback", feedbackId.ToString(), "not found", "Id"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            return new BaseModel<FeedbackModel>
            {
                Message = MessageResponseHelper.GetSuccessfully("feedback"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = new FeedbackModel
                {
                    Feedback = _mapper.Map<FeedbackResponseDTO>(feedback),
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<FeedbackModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<CreateFeedbackResponseModel, CreateFeedbackRequestModel>> CreateFeedback(CreateFeedbackRequestModel request)
    {
        try
        {
            var newFeedback = new Feedback
            {
                Id = Guid.NewGuid(),
                MeetingId = request.MeetingId,
                UserId = request.UserId,
                Message = request.Message,
            };
            var addResult = await _feedbackRepository.CreateAsync(newFeedback);
            if(addResult)
                return new BaseModel<CreateFeedbackResponseModel, CreateFeedbackRequestModel>
                {
                    Message = MessageResponseHelper.CreateSuccessfully("feedback"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    RequestModel = request,
                    ResponseModel = new CreateFeedbackResponseModel
                    {
                        FeedbackId = newFeedback.Id.ToString()
                    }
                };
            return new BaseModel<CreateFeedbackResponseModel, CreateFeedbackRequestModel>
            {
                Message = MessageResponseHelper.CreateFailed("feedback"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status200OK,
            };
        }
        catch (Exception e)
        {
            return new BaseModel<CreateFeedbackResponseModel, CreateFeedbackRequestModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<FeedbackModel>> UpdateFeedback(Guid feedbackId, string message)
    {
        try
        {
            var feedback = await _feedbackRepository.GetByIdAsync(feedbackId, "Id");
            if (feedback == null)
                return new BaseModel<FeedbackModel>
                {
                    Message = MessageResponseHelper.DetailException("feedback", feedbackId.ToString(), "not found", "Id"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            feedback.Message = message;
            var updateResult =_feedbackRepository.Update(feedback);
            if (updateResult)
                return new BaseModel<FeedbackModel>
                {
                    Message = MessageResponseHelper.UpdateSuccessfully("feedback"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    ResponseRequestModel = new FeedbackModel
                    {
                        Feedback = _mapper.Map<FeedbackResponseDTO>(feedback),
                    }
                };
            return new BaseModel<FeedbackModel>
            {
                Message = MessageResponseHelper.UpdateFailed("feedback"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status200OK,
            };
        }
        catch (Exception e)
        {
            return new BaseModel<FeedbackModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<Pagination<FeedbackByMentorDTO>>> GetFeedbackByMentorId(string mentorId, int page, int size)
    {
        var result = await _feedbackRepository.GetFeedBacksByMentorId(mentorId, page, size);

        var feedbackDtoList = new List<FeedbackByMentorDTO>();
        foreach (var item in result.Items)
        {
            var feedbackDTO = new FeedbackByMentorDTO
            {
                Username = item.User.UserName,
                Message = item.Message,
                UpdatedBy = item.UpdatedBy,
                CreatedBy = item.CreatedBy,
                CreatedOn = item.CreatedOn,
                UpdatedOn = item.UpdatedOn
            };
            feedbackDtoList.Add(feedbackDTO);
        }

        var paginatedDtoList = new Pagination<FeedbackByMentorDTO>
        {
            Items = feedbackDtoList,
            PageIndex = page,
            PageSize = size
        };

        return new BaseModel<Pagination<FeedbackByMentorDTO>>
        {
            Message = MessageResponseHelper.GetSuccessfully("feedback"),
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            ResponseRequestModel = paginatedDtoList
        };
    }



    public async Task<BaseModel<Pagination<FeedbackResponseDTO>>> GetAllFeedbacks(int page, int size)
    {
        var result = await _feedbackRepository.GetPagedListAsync(page, size);

        var feedbackDtoList = new List<FeedbackResponseDTO>();
        foreach (var item in result.Items) {
            var mentor = await _mentorRepository.GetByUserIdAsync(item.UserId, include: m => m.Include(m => m.User));
            var feedbackToList = new FeedbackResponseDTO
            {
                MeetingId = item.MeetingId,
                name = mentor.User.FullName,
                Message = item.Message,
                UpdatedBy = item.UpdatedBy,
                CreatedBy = item.CreatedBy,
                CreatedOn = item.CreatedOn,
                UpdatedOn = item.UpdatedOn
            };
            feedbackDtoList.Add(feedbackToList);
        }

        var paginatedDtoList = new Pagination<FeedbackResponseDTO>
        {
            Items = feedbackDtoList,
            PageIndex = page,
            PageSize = size
        };

        return new BaseModel<Pagination<FeedbackResponseDTO>>
        {
            Message = MessageResponseHelper.GetSuccessfully("feedback"),
            IsSuccess = true,
            StatusCode = StatusCodes.Status200OK,
            ResponseRequestModel = paginatedDtoList
        };
    }

}