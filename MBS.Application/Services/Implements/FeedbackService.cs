using MBS.Application.Helpers;
using MBS.Application.Models.Feedback;
using MBS.Application.Models.General;
using MBS.Application.Models.Meeting;
using MBS.Application.Services.Interfaces;
using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace MBS.Application.Services.Implements;

public class FeedbackService : IFeedbackService
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IMeetingRepository _meetingRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public FeedbackService(IFeedbackRepository feedbackRepository, IMeetingRepository meetingRepository, UserManager<ApplicationUser> userManager)
    {
        _feedbackRepository = feedbackRepository;
        _meetingRepository = meetingRepository;
        _userManager = userManager;
    }
    public async Task<BaseModel<GetFeedbackRequestModel>> GetFeedbacksByUserId(Guid meetingId, string userId)
    {
        try
        {
            //check meeting
            var meeting = await _meetingRepository.GetAsync(m => m.Id == meetingId);
            if (meeting == null)
                return new BaseModel<GetFeedbackRequestModel>
                {
                    Message = MessageResponseHelper.DetailException("meeting", meetingId.ToString(), "not found", "Id"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new BaseModel<GetFeedbackRequestModel>
                {
                    Message = MessageResponseHelper.DetailException("student", userId, "not found", "Id"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            //get all
            var feedbacks =
                await _feedbackRepository.GetAllAsync(f => f.MeetingId == meetingId && f.UserId == userId);
            return new BaseModel<GetFeedbackRequestModel>
            {
                Message = MessageResponseHelper.GetSuccessfully("feedbacks"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = new GetFeedbackRequestModel
                {
                    Feedbacks = feedbacks
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<GetFeedbackRequestModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<GetFeedbackRequestModel>> GetFeedbacksByMeetingId(Guid meetingId)
    {
        try {
            //check meeting
            var meeting = await _meetingRepository.GetAsync(m => m.Id == meetingId);
            if (meeting == null)
                return new BaseModel<GetFeedbackRequestModel>
                {
                    Message = MessageResponseHelper.DetailException("meeting", meetingId.ToString(), "not found", "Id"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            //get all
            var feedbacks =
                await _feedbackRepository.GetAllAsync(f => f.MeetingId == meetingId, "Student");
            return new BaseModel<GetFeedbackRequestModel>
            {
                Message = MessageResponseHelper.GetSuccessfully("feedbacks"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = new GetFeedbackRequestModel
                {
                    Feedbacks = feedbacks
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<GetFeedbackRequestModel>
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
                await _feedbackRepository.GetAsync(f => f.Id == feedbackId);
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
                    Feedback = feedback
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
            var addResult = await _feedbackRepository.AddAsync(newFeedback);
            if(addResult)
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
            return new BaseModel<CreateFeedbackResponseModel, CreateFeedbackRequestModel>
            {
                Message = MessageResponseHelper.GetSuccessfully("feedback"),
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
            var feedback = await _feedbackRepository.GetAsync(f => f.Id == feedbackId);
            if (feedback == null)
                return new BaseModel<FeedbackResponseModel>
                {
                    Message = MessageResponseHelper.DetailException("feedback", feedbackId.ToString(), "not found", "Id"),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            feedback.Message = message;
            var updateResult = await _feedbackRepository.UpdateAsync(feedback);
            if (updateResult)
                return new BaseModel<FeedbackResponseModel>
                {
                    Message = MessageResponseHelper.UpdateSuccessfully("feedback"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    ResponseRequestModel = new FeedbackResponseModel
                    {
                        Feedback = feedback
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