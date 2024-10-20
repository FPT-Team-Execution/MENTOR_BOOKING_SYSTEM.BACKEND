using System.Linq.Expressions;
using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.Feedback;
using MBS.Application.Models.General;
using MBS.Application.Models.Meeting;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO;
using MBS.DataAccess.Repositories;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class FeedbackService : BaseService<FeedbackService>, IFeedbackService
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IMeetingRepository _meetRepository;
    public FeedbackService(IUnitOfWork unitOfWork, IMeetingRepository meetingRepository, IFeedbackRepository feedbackRepository, ILogger<FeedbackService> logger, UserManager<ApplicationUser> userManager, IMapper mapper) : base(unitOfWork, logger, mapper)
    {
       _meetRepository = meetingRepository;
        _userManager = userManager;
        _feedbackRepository = feedbackRepository;
    }

    public async Task<BaseModel<Pagination<FeedbackResponseDto>>> GetFeedbacks(int page, int size, DateTime? startDate, DateTime? endDate)
    {
        try
        {
            if (endDate != null && startDate != null && endDate < startDate)
            {
                return new BaseModel<Pagination<FeedbackResponseDto>>
                {
                    Message = MessageResponseHelper.InvalidInputParameterDetail("start and end time"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                };
            }
            //get all
            Expression<Func<Feedback, bool>> feedBackExpression = null; 
            Expression<Func<Feedback, bool>> startExpression = f => f.CreatedOn >= startDate;
            Expression<Func<Feedback, bool>> endExpression = f => f.CreatedOn < endDate;

            if (startDate != null && endDate != null)
                feedBackExpression = Expression.Lambda<Func<Feedback, bool>>(Expression.AndAlso(startExpression.Body, endExpression.Body));
            else if (endDate != null)
                feedBackExpression = endExpression;
            else if (startDate != null)
                feedBackExpression = startExpression;

            var feedbacks =
                await _feedbackRepository.GetFeedbackPagingAsync(
                    predicate: feedBackExpression,
                    page: page, 
                    size: size
                );
            
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

    public async Task<BaseModel<Pagination<FeedbackResponseDto>>> GetFeedbacksByUserId(Guid meetingId, string userId, int page, int size)
    {
        try
        {
            //check meeting
            var meeting = await _meetRepository.GetMeetingByIdAsync(meetingId);
            if (meeting == null)
                return new BaseModel<Pagination<FeedbackResponseDto>>
                {
                    Message = MessageResponseHelper.DetailException("meeting", meetingId.ToString(), "not found", "Id"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new BaseModel<Pagination<FeedbackResponseDto>>
                {
                    Message = MessageResponseHelper.DetailException("student", userId, "not found", "Id"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            //get all
            var feedbacks =
                await _feedbackRepository.GetFeedbackPagingAsync(
                    page, size, f => f.MeetingId == meetingId && f.UserId == userId
                    
                    );
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

    public async Task<BaseModel<Pagination<FeedbackResponseDto>>> GetFeedbacksByMeetingId(Guid meetingId, int page, int size)
    {
        try {
            //check meeting
            var meeting = await _meetRepository.GetMeetingByIdAsync(meetingId);
            if (meeting == null)
                return new BaseModel<Pagination<FeedbackResponseDto>>
                {
                    Message = MessageResponseHelper.DetailException("meeting", meetingId.ToString(), "not found", "Id"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            //get all
            var feedbacks =
                await _feedbackRepository.GetFeedbackPagingAsync(
                    page: page,
                    size: size,
                    predicate: f => f.MeetingId == meetingId,
                    include: f => f.Include(x => x.User)
                    
                    );
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
            var feedback =
                await _feedbackRepository.GetFeedbackByIdAsync(feedbackId);
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
            await _feedbackRepository.CreateAsync(newFeedback); 
                return new BaseModel<CreateFeedbackResponseModel, CreateFeedbackRequestModel>
                {
                    Message = MessageResponseHelper.GetSuccessfully("feedback"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    RequestModel = request,
                    ResponseModel = new CreateFeedbackResponseModel
                    {
                        FeedbackId = newFeedback.Id.ToString()
                    }
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
            var feedback = await _feedbackRepository.GetFeedbackByIdAsync(feedbackId);
            if (feedback == null)
                return new BaseModel<FeedbackResponseModel>
                {
                    Message = MessageResponseHelper.DetailException("feedback", feedbackId.ToString(), "not found", "Id"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            feedback.Message = message;
            _feedbackRepository.Update(feedback);
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