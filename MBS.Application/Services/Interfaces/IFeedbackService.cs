using MBS.Application.Models.Feedback;
using MBS.Application.Models.General;
using MBS.Application.Models.Meeting;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Implements;

namespace MBS.Application.Services.Interfaces;

public interface IFeedbackService
{
    Task<BaseModel<Pagination<FeedbackResponseDto>>> GetFeedbacksByUserId(Guid meetingId, string userId, int page, int size);
    Task<BaseModel<Pagination<FeedbackResponseDto>>> GetFeedbacksByMeetingId(Guid meetingId, int page, int size);
    Task<BaseModel<FeedbackResponseModel>> GetFeedbackById(Guid feedbackId);
    Task<BaseModel<CreateFeedbackResponseModel, CreateFeedbackRequestModel>> CreateFeedback(CreateFeedbackRequestModel request);
    Task<BaseModel<FeedbackResponseModel>> UpdateFeedback(Guid feedbackId, string message);

}