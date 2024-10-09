using MBS.Application.Models.General;
using MBS.Application.Models.Positions;
using MBS.Core.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Services.Interfaces
{
    public interface IPositionService
    {
        Task<BaseModel<CreatePositionResponseModel, CreatePositionRequestModel>> CreateNewPosition(
        CreatePositionRequestModel request);

        Task<BaseModel<PositionModel>> GetPositionId(
            Guid requestId);

        Task<BaseModel<PositionModel>> UpdatePosition(Guid id, UpdatePositionRequestModel request);

        Task<BaseModel> RemovePosition(
            Guid id);
        Task<BaseModel<Pagination<PositionResponseDTO>>> GetPositions(int page, int size);
    }
}
