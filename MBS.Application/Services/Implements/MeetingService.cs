using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Meeting;
using MBS.Application.Models.PointTransaction;
using MBS.Application.Models.Student;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class MeetingService : BaseService2<MeetingService>, IMeetingService
{
    private readonly IMeetingRepository _meetingRepository;
    private readonly IRequestRepository _requestRepository;
    private readonly IPointTransactionSerivce _pointTransactionService;
    private readonly IProjectRepository _projectRepository;
    private readonly IGroupRepository _groupRepository;

    public MeetingService(
        IRequestRepository requestRepository,
        IMeetingRepository meetingRepository,
        ILogger<MeetingService> logger, IMapper mapper, IPointTransactionSerivce pointTransactionService,
        IProjectRepository projectRepository, IGroupRepository groupRepository) : base(logger,
        mapper)
    {
        _meetingRepository = meetingRepository;
        _pointTransactionService = pointTransactionService;
        _projectRepository = projectRepository;
        _groupRepository = groupRepository;
        _requestRepository = requestRepository;
    }

    public async Task<BaseModel<MeetingResponseModel>> GetMeetingId(Guid meetingId)
    {
        try
        {
            var meeting = await _meetingRepository.GetByIdAsync(meetingId, "Id");
            if (meeting == null)
                return new BaseModel<MeetingResponseModel>
                {
                    Message = MessageResponseHelper.MeetingNotFound(meetingId.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            return new BaseModel<MeetingResponseModel>
            {
                Message = MessageResponseHelper.GetSuccessfully("meeting"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = new MeetingResponseModel
                {
                    Meeting = _mapper.Map<MeetingResponseDto>(meeting),
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<MeetingResponseModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<Pagination<MeetingResponseDto>>> GetMeetings(int page, int size)
    {
        try
        {
            var meetings = await _meetingRepository.GetPagedListAsync(page, size);
            return new BaseModel<Pagination<MeetingResponseDto>>
            {
                Message = MessageResponseHelper.GetSuccessfully("meetings"),
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                ResponseRequestModel = _mapper.Map<Pagination<MeetingResponseDto>>(meetings)
            };
        }
        catch (Exception e)
        {
            return new BaseModel<Pagination<MeetingResponseDto>>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<CreateMeetingResponseModel, CreateMeetingRequestModel>> CreateMeeting(
        CreateMeetingRequestModel request)
    {
        try
        {
            //check request
            var requestCheck = await _requestRepository.GetByIdAsync(request.RequestId, "Id");
            if (requestCheck == null)
                return new BaseModel<CreateMeetingResponseModel, CreateMeetingRequestModel>
                {
                    Message = MessageResponseHelper.RequestNotFound(request.RequestId.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };

            if (requestCheck.Status != RequestStatusEnum.Accepted)
                return new BaseModel<CreateMeetingResponseModel, CreateMeetingRequestModel>
                {
                    Message = MessageResponseHelper.InvalidRequestStatus(request.RequestId.ToString(),
                        nameof(RequestStatusEnum.Accepted)),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                };

            //Create meeting
            var newMeeting = new Meeting()
            {
                Id = Guid.NewGuid(),
                RequestId = request.RequestId,
                Description = request.Description,
                Location = request.Location,
                MeetUp = request.MeetUp,
                Status = MeetingStatusEnum.New
            };
            var addResult = await _meetingRepository.CreateAsync(newMeeting);
            if (addResult)
                return new BaseModel<CreateMeetingResponseModel, CreateMeetingRequestModel>
                {
                    Message = MessageResponseHelper.GetSuccessfully("meeting"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    RequestModel = request,
                    ResponseModel = new CreateMeetingResponseModel
                    {
                        RequestId = newMeeting.Id,
                    }
                };

            switch (requestCheck.ProjectId)
            {
                case null:
                {
                    var transactionResult = await _pointTransactionService.ModifyStudentPoint(
                        new ModifyStudentPointRequestModel()
                        {
                            Amout = 100,
                            StudentId = requestCheck.CreaterId,
                            TransactionType = TransactionTypeEnum.Debit
                        });
                    break;
                }
                default:
                {
                    var project = await _projectRepository.GetByIdAsync(requestCheck.ProjectId, "Id");
                    var groups = await _groupRepository.GetGroupByProjectIdAsync(project.Id);

                    foreach (var group in groups)
                    {
                        var transactionResult = await _pointTransactionService.ModifyStudentPoint(
                            new ModifyStudentPointRequestModel()
                            {
                                Amout = 100,
                                StudentId = group.StudentId,
                                TransactionType = TransactionTypeEnum.Debit
                            });
                    }
                    break;
                }
            }

            return new BaseModel<CreateMeetingResponseModel, CreateMeetingRequestModel>
            {
                Message = MessageResponseHelper.CreateFailed("meeting"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
        catch (Exception e)
        {
            return new BaseModel<CreateMeetingResponseModel, CreateMeetingRequestModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }

    public async Task<BaseModel<MeetingResponseModel>> UpdateMeeting(Guid meetingId, UpdateMeetingRequestModel request)
    {
        try
        {
            //check meeting
            var meeting = await _meetingRepository.GetByIdAsync(meetingId, "Id");
            if (meeting == null)
                return new BaseModel<MeetingResponseModel>
                {
                    Message = MessageResponseHelper.MeetingNotFound(meetingId.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                };
            if (meeting.Status != MeetingStatusEnum.New || meeting.Status != MeetingStatusEnum.Delayed)
                return new BaseModel<MeetingResponseModel>
                {
                    Message = MessageResponseHelper.InvalidMeetingSatus(meetingId.ToString()),
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                };

            //Update request
            meeting.Description = request.Description;
            meeting.Location = request.Location;
            meeting.MeetUp = meeting.MeetUp;
            meeting.Status = Enum.Parse<MeetingStatusEnum>(request.Status, true);
            var updateResult = _meetingRepository.Update(meeting);
            if (updateResult)
                return new BaseModel<MeetingResponseModel>
                {
                    Message = MessageResponseHelper.UpdateSuccessfully("meeting"),
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    ResponseRequestModel = new MeetingResponseModel()
                    {
                        Meeting = _mapper.Map<MeetingResponseDto>(meeting),
                    }
                };
            return new BaseModel<MeetingResponseModel>
            {
                Message = MessageResponseHelper.UpdateFailed("meeting"),
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
        catch (Exception e)
        {
            return new BaseModel<MeetingResponseModel>
            {
                Message = e.Message,
                IsSuccess = false,
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}