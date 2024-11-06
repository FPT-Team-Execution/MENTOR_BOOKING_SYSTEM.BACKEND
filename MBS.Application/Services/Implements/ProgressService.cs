using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Progress;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MBS.Application.Services.Implements;

public class ProgressService : BaseService2<ProgressService>, IProgressService
{
    private readonly IProgressRepository _progressRepository;
    private readonly IProjectRepository _projectRepository;

    public ProgressService(
        IProjectRepository projectRepository, IProgressRepository progressRepository,
        ILogger<ProgressService> logger, IMapper mappger) : base(logger, mappger)
    {
        _projectRepository = projectRepository;
        _progressRepository = progressRepository;
    }

    public async Task<BaseModel<GetProgressByProjectIddResponse>> GetProgressesByProjectId(
        GetProgressByProjectIddRequest request)
    {
        try
        {
            //* check project 
            var project = await _projectRepository.GetByIdAsync(request.ProjectId, "Id");
            if (project == null)
                return new BaseModel<GetProgressByProjectIddResponse>()
                {
                    Message = MessageResponseHelper.ProjectNotFound(request.ProjectId.ToString()),
                    StatusCode = StatusCodes.Status404NotFound,
                    IsSuccess = false,
                };

            //* get progress and sort By create Time
            var progress =
                await _progressRepository.GetProgressesAsync(request.ProjectId, request.Page, request.Size, "asc");
            return new BaseModel<GetProgressByProjectIddResponse>()
            {
                Message = MessageResponseHelper.ProjectNotFound(request.ProjectId.ToString()),
                StatusCode = StatusCodes.Status404NotFound,
                IsSuccess = false,
                ResponseRequestModel = new GetProgressByProjectIddResponse
                {
                    Progresses = _mapper.Map<Pagination<ProgressResponseDto>>(progress),
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<GetProgressByProjectIddResponse>()
            {
                Message = MessageResponseHelper.ProjectNotFound(e.Message),
                StatusCode = StatusCodes.Status500InternalServerError,
                IsSuccess = false,
            };
        }
    }

    public async Task<BaseModel<CreateProgressResponse>> CreateProgress(CreateProgressRequest request)
    {
        try
        {
            //* check project 
            var project = await _projectRepository.GetByIdAsync(request.ProjectId, "Id");
            if (project == null)
                return new BaseModel<CreateProgressResponse>()
                {
                    Message = MessageResponseHelper.ProjectNotFound(request.ProjectId.ToString()),
                    StatusCode = StatusCodes.Status404NotFound,
                    IsSuccess = false,
                };
            //* check ~ only project activated to create 
            if (project.Status != ProjectStatusEnum.Activated)
                return new BaseModel<CreateProgressResponse>()
                {
                    Message = MessageResponseHelper.ProjectClosed(request.ProjectId.ToString()),
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false,
                };

            //* get progress and sort By create Time
            var progressCreate = new Progress
            {
                Id = request.ProjectId,
                Name = request.Name,
                ProjectId = request.ProjectId,
                IsComplete = false,
            };
            var addResult = await _progressRepository.CreateAsync(progressCreate);
            if (!addResult)
                return new BaseModel<CreateProgressResponse>()
                {
                    Message = MessageResponseHelper.CreateFailed("progress"),
                    StatusCode = StatusCodes.Status500InternalServerError,
                    IsSuccess = false
                };
            return new BaseModel<CreateProgressResponse>()
            {
                Message = MessageResponseHelper.CreateSuccessfully("progress"),
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = true,
                ResponseRequestModel = new CreateProgressResponse
                {
                    ProgressId = progressCreate.Id
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<CreateProgressResponse>()
            {
                Message = e.Message,
                StatusCode = StatusCodes.Status500InternalServerError,
                IsSuccess = false
            };
        }
    }

    public async Task<BaseModel<UpdateProgressResponse>> UpdateProgress(UpdateProgressRequest request)
    {
        try
        {
            //* check progress
            var progress = await _progressRepository.GetProgressByIdAsync(request.ProgressId);
            if (progress == null)
                return new BaseModel<UpdateProgressResponse>()
                {
                    Message = MessageResponseHelper.NotFound("progress"),
                    StatusCode = StatusCodes.Status404NotFound,
                    IsSuccess = false,
                };
            //* check progress project
            if (progress.Project.Status != ProjectStatusEnum.Activated)
                return new BaseModel<UpdateProgressResponse>()
                {
                    Message = MessageResponseHelper.ProjectClosed(progress.ProjectId.ToString()),
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false,
                };

            //* Update
            progress.Name = request.Name;
            progress.IsComplete = request.IsComplete;
            var updateResult = _progressRepository.Update(progress);
            if (!updateResult)
                return new BaseModel<UpdateProgressResponse>()
                {
                    Message = MessageResponseHelper.UpdateFailed("progress"),
                    StatusCode = StatusCodes.Status500InternalServerError,
                    IsSuccess = false
                };
            return new BaseModel<UpdateProgressResponse>()
            {
                Message = MessageResponseHelper.CreateSuccessfully("progress"),
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = true,
                ResponseRequestModel = new UpdateProgressResponse
                {
                    Progress = _mapper.Map<ProgressResponseDto>(progress)
                }
            };
        }
        catch (Exception e)
        {
            return new BaseModel<UpdateProgressResponse>()
            {
                Message = e.Message,
                StatusCode = StatusCodes.Status500InternalServerError,
                IsSuccess = false
            };
        }
    }

    public async Task<BaseModel> DeleteProgress(DeleteProgressRequest request)
    {
        try
        {
            //* check progress
            var progress = await _progressRepository.GetProgressByIdAsync(request.ProgressId);
            if (progress == null)
                return new BaseModel()
                {
                    Message = MessageResponseHelper.NotFound("progress"),
                    StatusCode = StatusCodes.Status404NotFound,
                    IsSuccess = false,
                };
            //* check progress project
            if (progress.Project.Status != ProjectStatusEnum.Activated)
                return new BaseModel()
                {
                    Message = MessageResponseHelper.ProjectClosed(progress.ProjectId.ToString()),
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false,
                };


            var deleteResult = _progressRepository.Delete(progress);

            return new BaseModel()
            {
                Message = deleteResult
                    ? MessageResponseHelper.DeleteSuccessfully("progress")
                    : MessageResponseHelper.DeleteFailed("progress"),
                StatusCode = deleteResult ? StatusCodes.Status200OK : StatusCodes.Status500InternalServerError,
                IsSuccess = deleteResult
            };
        }
        catch (Exception e)
        {
            return new BaseModel()
            {
                Message = e.Message,
                StatusCode = StatusCodes.Status500InternalServerError,
                IsSuccess = false
            };
        }
    }
}