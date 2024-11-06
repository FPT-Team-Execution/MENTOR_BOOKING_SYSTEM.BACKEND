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
    Task<BaseModel<Pagination<FeedbackResponseDTO>>> GetMeetingFeedbacksByUserId(GetMeetingFeedbacksByUserIdRequest request);
    Task<BaseModel<Pagination<FeedbackResponseDTO>>> GetFeedbacksByMeetingId(GetFeedbacksByMeetingIdRequest request);
    Task<BaseModel<FeedbackModel>> GetFeedbackById(Guid feedbackId);
    Task<BaseModel<CreateFeedbackResponseModel, CreateFeedbackRequestModel>> CreateFeedback(CreateFeedbackRequestModel request);
    Task<BaseModel<FeedbackModel>> UpdateFeedback(Guid feedbackId, string message);
    Task<BaseModel<Pagination<FeedbackByMentorDTO>>> GetFeedbackByMentorId(string mentorId, int page, int size);
    Task<BaseModel<Pagination<FeedbackResponseDTO>>> GetAllFeedbacks(int page, int size);




}