using MBS.Application.Models.Feedback;
using MBS.Application.Models.General;
using MBS.Application.Models.Meeting;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Implements;

namespace MBS.Application.Services.Interfaces;

public interface IFeedbackService
{
    // Task<BaseModel<Pagination<FeedbackResponseDto>>> GetFeedbacks(int page, int size, DateTime? startDate, DateTime? endDate);
    Task<BaseModel<Pagination<FeedbackResponseDto>>> GetMeetingFeedbacksByUserId(GetMeetingFeedbacksByUserIdRequest request);
    Task<BaseModel<Pagination<FeedbackResponseDto>>> GetFeedbacksByMeetingId(GetFeedbacksByMeetingIdRequest request);
    Task<BaseModel<FeedbackResponseModel>> GetFeedbackById(Guid feedbackId);
    Task<BaseModel<CreateFeedbackResponseModel, CreateFeedbackRequestModel>> CreateFeedback(CreateFeedbackRequestModel request);
    Task<BaseModel<FeedbackResponseModel>> UpdateFeedback(Guid feedbackId, string message);

}