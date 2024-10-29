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
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class FeedbackService : BaseService2<FeedbackService>, IFeedbackService
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMeetingRepository _meetingRepository;
    private readonly IFeedbackRepository _feedbackRepository;

    public FeedbackService(
        IFeedbackRepository feedbackRepository,
        IMeetingRepository meetingRepository,
        UserManager<ApplicationUser> userManager,
        ILogger<FeedbackService> logger,
        IMapper mapper) : base(logger, mapper)
    {
        _feedbackRepository = feedbackRepository;
        _meetingRepository = meetingRepository;
        _userManager = userManager;
    }

    // public async Task<BaseModel<Pagination<FeedbackResponseDto>>> GetFeedbacks(int page, int size, DateTime? startDate, DateTime? endDate)
    // {
    //     try
    //     {
    //         if (endDate != null && startDate != null && endDate < startDate)
    //         {
    //             return new BaseModel<Pagination<FeedbackResponseDto>>
    //             {
    //                 Message = MessageResponseHelper.InvalidInputParameterDetail("start and end time"),
    //                 IsSuccess = false,
    //                 StatusCode = StatusCodes.Status400BadRequest,
    //             };
    //         }
    //         //get all
    //         Expression<Func<Feedback, bool>> feedBackExpression = null; 
    //         Expression<Func<Feedback, bool>> startExpression = f => f.CreatedOn >= startDate;
    //         Expression<Func<Feedback, bool>> endExpression = f => f.CreatedOn < endDate;
    //
    //         if (startDate != null && endDate != null)
    //             feedBackExpression = Expression.Lambda<Func<Feedback, bool>>(Expression.AndAlso(startExpression.Body, endExpression.Body));
    //         else if (endDate != null)
    //             feedBackExpression = endExpression;
    //         else if (startDate != null)
    //             feedBackExpression = startExpression;
    //
    //         var feedbacks =
    //             await _unitOfWork.GetRepository<Feedback>().GetPagingListAsync(
    //                 predicate: feedBackExpression,
    //                 page: page, 
    //                 size: size
    //             );
    //         
    //         return new BaseModel<Pagination<FeedbackResponseDto>>
    //         {
    //             Message = MessageResponseHelper.GetSuccessfully("feedbacks"),
    //             IsSuccess = true,
    //             StatusCode = StatusCodes.Status200OK,
    //             ResponseRequestModel = _mapper.Map<Pagination<FeedbackResponseDto>>(feedbacks)
    //         };
    //     }
    //     catch (Exception e)
    //     {
    //         return new BaseModel<Pagination<FeedbackResponseDto>>
    //         {
    //             Message = e.Message,
    //             IsSuccess = false,
    //             StatusCode = StatusCodes.Status500InternalServerError,
    //         };
    //     }
    // }

    public async Task<BaseModel<Pagination<FeedbackResponseDto>>> GetMeetingFeedbacksByUserId(GetMeetingFeedbacksByUserIdRequest request)
    {
        try
        {
            //check meeting
            var meeting = await _meetingRepository.GetByIdAsync(request.MeetingId, "Id");
            if (meeting == null)
                return new BaseModel<Pagination<FeedbackResponseDto>>
                {
                    Message = MessageResponseHelper.DetailException("meeting", request.MeetingId.ToString(), "not found", "Id"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return new BaseModel<Pagination<FeedbackResponseDto>>
                {
                    Message = MessageResponseHelper.DetailException("student", request.UserId, "not found", "Id"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            //get all
            var feedbacks =
                await _feedbackRepository.GetMeetingFeedBacksByUserId(request.MeetingId, request.UserId, request.Page, request.Size, request.SortOrder);
            return new BaseModel<Pagination<FeedbackResponseDto>>
            {
                Message = MessageResponseHelper.GetSuccessfully("feedbacks"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = _mapper.Map<Pagination<FeedbackResponseDto>>(feedbacks)
			};
        }
        catch (Exception e)
        {
            return new BaseModel<Pagination<FeedbackResponseDto>>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<Pagination<FeedbackResponseDto>>> GetFeedbacksByMeetingId(GetFeedbacksByMeetingIdRequest request)
    {
        try {
            //check meeting
            var meeting = await _meetingRepository.GetByIdAsync(request.MeetingId, "Id");
            if (meeting == null)
                return new BaseModel<Pagination<FeedbackResponseDto>>
                {
                    Message = MessageResponseHelper.DetailException("meeting", request.MeetingId.ToString(), "not found", "Id"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            //get all
            var feedbacks =
                await _feedbackRepository.GetFeedBacksByMeetingId(request.MeetingId, request.Page, request.Size, request.SortOrder);
            return new BaseModel<Pagination<FeedbackResponseDto>>
            {
                Message = MessageResponseHelper.GetSuccessfully("feedbacks"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = _mapper.Map<Pagination<FeedbackResponseDto>>(feedbacks)
			};
        }
        catch (Exception e)
        {
            return new BaseModel<Pagination<FeedbackResponseDto>>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<FeedbackResponseModel>> GetFeedbackById(Guid feedbackId)
    {
        try {
            var feedback = await _feedbackRepository.GetByIdAsync(feedbackId, "Id");
            if (feedback == null)
                return new BaseModel<FeedbackResponseModel>
                {
                    Message = MessageResponseHelper.DetailException("feedback", feedbackId.ToString(), "not found", "Id"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            return new BaseModel<FeedbackResponseModel>
            {
                Message = MessageResponseHelper.GetSuccessfully("feedback"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = new FeedbackResponseModel
                {
                    Feedback = _mapper.Map<FeedbackResponseDto>(feedback),
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<FeedbackResponseModel>
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

    public async Task<BaseModel<FeedbackResponseModel>> UpdateFeedback(Guid feedbackId, string message)
    {
        try
        {
            var feedback = await _feedbackRepository.GetByIdAsync(feedbackId, "Id");
            if (feedback == null)
                return new BaseModel<FeedbackResponseModel>
                {
                    Message = MessageResponseHelper.DetailException("feedback", feedbackId.ToString(), "not found", "Id"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            feedback.Message = message;
            var updateResult =_feedbackRepository.Update(feedback);
            if (updateResult)
                return new BaseModel<FeedbackResponseModel>
                {
                    Message = MessageResponseHelper.UpdateSuccessfully("feedback"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    ResponseRequestModel = new FeedbackResponseModel
                    {
                        Feedback = _mapper.Map<FeedbackResponseDto>(feedback),
                    }
                };
            return new BaseModel<FeedbackResponseModel>
            {
                Message = MessageResponseHelper.UpdateFailed("feedback"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status200OK,
            };
        }
        catch (Exception e)
        {
            return new BaseModel<FeedbackResponseModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}