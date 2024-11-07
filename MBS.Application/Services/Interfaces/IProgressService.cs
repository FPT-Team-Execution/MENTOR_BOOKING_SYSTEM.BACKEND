using MBS.Application.Models.General;
using MBS.Application.Models.Majors;
using MBS.Application.Models.Progress;
using MBS.Core.Common.Pagination;

namespace MBS.Application.Services.Interfaces;

public interface IProgressService
{
    Task<BaseModel<GetCompleteProgressResponse>> GetCompleteProgressPercent(GetCompleteProgressRequest request);

    Task<BaseModel<GetProgressByProjectIddResponse>> GetProgressesByProjectId(GetProgressByProjectIddRequest request);
    Task<BaseModel<CreateProgressResponse>> CreateProgress(CreateProgressRequest request);
    // Task<BaseModel<CreateProgressesResponse>> CreateProgresses(CreateProgressesRequest request);

    Task<BaseModel<UpdateProgressResponse>> UpdateProgress(UpdateProgressRequest request);
    Task<BaseModel> DeleteProgress(DeleteProgressRequest request);
    
}