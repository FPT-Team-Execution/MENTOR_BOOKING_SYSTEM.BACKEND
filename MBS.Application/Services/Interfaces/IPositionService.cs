using MBS.Application.Models.General;
using MBS.Application.Models.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Services.Interfaces
{
	public interface IPositionService
	{
		Task<BaseModel<CreatePositionResponseModel, CreatePositionRequestModel>> CreateNewPositionAsync(
		CreatePositionRequestModel request);

		Task<BaseModel<GetPositionResponseModel, GetPositionRequestModel>> GetPosition(
			GetPositionRequestModel request);

		Task<BaseModel<UpdatePositionResponseModel, UpdatePositionRequestModel>> UpdatePosition(Guid id, UpdatePositionRequestModel request);

		Task<BaseModel<RemovePositionResponseModel, RemovePositionRequestModel>> RemovePosition(
			RemovePositionRequestModel request);
		Task<BaseModel<GetAllPositionResponseModel, GetAllPositionRequestModel>> GetAllPosition();
	}
}
