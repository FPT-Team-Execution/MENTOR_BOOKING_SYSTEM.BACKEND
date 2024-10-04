using MBS.Application.Models.Feedback;
using MBS.Application.Models.General;
using MBS.Application.Models.Meeting;
using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Implements;

namespace MBS.Application.Services.Interfaces;

public interface IFeedbackService
{
    Task<BaseModel<GetFeedbackRequestModel>> GetFeedbacksByUserId(Guid meetingId, string userId);
    Task<BaseModel<GetFeedbackRequestModel>> GetFeedbacksByMeetingId(Guid meetingId);
    Task<BaseModel<FeedbackResponseModel>> GetFeedbackById(Guid feedbackId);
    Task<BaseModel<CreateFeedbackResponseModel, CreateFeedbackRequestModel>> CreateFeedback(CreateFeedbackRequestModel request);
    Task<BaseModel<FeedbackResponseModel>> UpdateFeedback(Guid feedbackId, string message);
}