using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Positions;
using MBS.Application.Services.Interfaces;
using MBS.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MBS.DataAccess.DAO;

namespace MBS.Application.Services.Implements
{
	public class PositionService : BaseService<PositionService>, IPositionService

	{
		public PositionService(IUnitOfWork unitOfWork, ILogger<PositionService> logger, IMapper mapper) : base(unitOfWork, logger, mapper)
		{
		}

		public async Task<BaseModel<CreatePositionResponseModel, CreatePositionRequestModel>> CreateNewPositionAsync(CreatePositionRequestModel request)
		{
			var newPosition = new Position()
			{
				Id = new Guid(),
				Name = request.name,
				Description = request.description,
			};
			try
			{
				await _unitOfWork.GetRepository<Position>().InsertAsync(newPosition);
				if (await _unitOfWork.CommitAsync() > 0)
					return new BaseModel<CreatePositionResponseModel, CreatePositionRequestModel>
					{
						Message = MessageResponseHelper.CreateSuccessfully("postion"),
						IsSuccess = true,
						StatusCode = StatusCodes.Status200OK,
						RequestModel = request,
						ResponseModel = new CreatePositionResponseModel
						{
							position = newPosition
						}
					};
				return new BaseModel<CreatePositionResponseModel, CreatePositionRequestModel>
				{
					Message = MessageResponseHelper.CreateFailed("position"),
					IsSuccess = false,
					StatusCode = StatusCodes.Status200OK,
					RequestModel = request,
					ResponseModel = null
				};
			}
			catch (Exception e)
			{
				return new BaseModel<CreatePositionResponseModel, CreatePositionRequestModel>
				{
					Message = e.Message,
					IsSuccess = false,
					StatusCode = StatusCodes.Status500InternalServerError,
				};
			}
		}

		public async Task<BaseModel<GetAllPositionResponseModel, GetAllPositionRequestModel>> GetAllPosition()
		{
			try
			{
				var listPosition = await _unitOfWork.GetRepository<Position>().GetListAsync();
				if (listPosition != null)
				{
					return new BaseModel<GetAllPositionResponseModel, GetAllPositionRequestModel>
					{
						Message = MessageResponseHelper.GetSuccessfully("postion"),
						IsSuccess = true,
						StatusCode = StatusCodes.Status200OK,
						RequestModel = null,
						ResponseModel = new GetAllPositionResponseModel
						{
							positions = listPosition.ToList()
						}
					};
				}
				return new BaseModel<GetAllPositionResponseModel, GetAllPositionRequestModel>
				{
					Message = MessageResponseHelper.GetFailed("postion"),
					IsSuccess = true,
					StatusCode = StatusCodes.Status200OK,
					RequestModel = null,
					ResponseModel = null
				};

			}
			catch (Exception e)
			{

				return new BaseModel<GetAllPositionResponseModel, GetAllPositionRequestModel>
				{
					Message = e.Message,
					IsSuccess = false,
					StatusCode = StatusCodes.Status500InternalServerError,
				};
			}
		}

		public async Task<BaseModel<GetPositionResponseModel, GetPositionRequestModel>> GetPosition(GetPositionRequestModel request)
		{
			try
			{
				var positionResult = await _unitOfWork.GetRepository<Position>().SingleOrDefaultAsync(a => a.Id == request.Id);
				if (positionResult != null)
				{
					return new BaseModel<GetPositionResponseModel, GetPositionRequestModel>
					{
						Message = MessageResponseHelper.GetSuccessfully("postion"),
						IsSuccess = true,
						StatusCode = StatusCodes.Status200OK,
						RequestModel = null,
						ResponseModel = new GetPositionResponseModel
						{
							detailPosition = positionResult
						}
					};
				}
				return new BaseModel<GetPositionResponseModel, GetPositionRequestModel>
				{
					Message = MessageResponseHelper.GetFailed("postion"),
					IsSuccess = true,
					StatusCode = StatusCodes.Status200OK,
					RequestModel = null,
					ResponseModel = null
				};

			}
			catch (Exception e)
			{

				return new BaseModel<GetPositionResponseModel, GetPositionRequestModel>
				{
					Message = e.Message,
					IsSuccess = false,
					StatusCode = StatusCodes.Status500InternalServerError,
				};
			}
		}

		public async Task<BaseModel<RemovePositionResponseModel, RemovePositionRequestModel>> RemovePosition(RemovePositionRequestModel request)
		{
			try
			{
				var selectedPosition = await _unitOfWork.GetRepository<Position>().SingleOrDefaultAsync(a => a.Id == request.Id);

				if (selectedPosition == null)
				{
					return new BaseModel<RemovePositionResponseModel, RemovePositionRequestModel>
					{
						Message = MessageResponseHelper.Fail("Remove position - Position not found"),
						IsSuccess = false,
						StatusCode = StatusCodes.Status404NotFound,
						RequestModel = request,
					};
				}

				_unitOfWork.GetRepository<Position>().DeleteAsync(selectedPosition);

				if (await _unitOfWork.CommitAsync() > 0)
				{
					return new BaseModel<RemovePositionResponseModel, RemovePositionRequestModel>
					{
						Message = MessageResponseHelper.Successfully("Remove position"),
						IsSuccess = true,
						StatusCode = StatusCodes.Status200OK,
						RequestModel = request,
					};
				}

				return new BaseModel<RemovePositionResponseModel, RemovePositionRequestModel>
				{
					Message = MessageResponseHelper.Fail("Remove position - Commit failed"),
					IsSuccess = false,
					StatusCode = StatusCodes.Status500InternalServerError,
					RequestModel = request,
				};
			}
			catch (Exception e)
			{
				return new BaseModel<RemovePositionResponseModel, RemovePositionRequestModel>
				{
					Message = e.Message,
					IsSuccess = false,
					StatusCode = StatusCodes.Status500InternalServerError,
				};
			}
		}


		public async Task<BaseModel<UpdatePositionResponseModel, UpdatePositionRequestModel>> UpdatePosition(Guid id, UpdatePositionRequestModel request)
		{
			try
			{
				// Tìm kiếm vị trí bằng Id
				var existingPosition = await _unitOfWork.GetRepository<Position>().SingleOrDefaultAsync(p => p.Id == id);

				if (existingPosition == null)
				{
					return new BaseModel<UpdatePositionResponseModel, UpdatePositionRequestModel>
					{
						Message = MessageResponseHelper.Fail("Position not found"),
						IsSuccess = false,
						StatusCode = StatusCodes.Status404NotFound,
						RequestModel = request,
						ResponseModel = null
					};
				}

				// Cập nhật thông tin vị trí
				existingPosition.Name = request.Name;
				existingPosition.Description = request.Description;

				_unitOfWork.GetRepository<Position>().UpdateAsync(existingPosition);

				if (await _unitOfWork.CommitAsync() > 0)
				{
					return new BaseModel<UpdatePositionResponseModel, UpdatePositionRequestModel>
					{
						Message = MessageResponseHelper.UpdateSuccessfully("position"),
						IsSuccess = true,
						StatusCode = StatusCodes.Status200OK,
						RequestModel = request,
						ResponseModel = new UpdatePositionResponseModel
						{
							updatedPosition = existingPosition
						}
					};
				}

				return new BaseModel<UpdatePositionResponseModel, UpdatePositionRequestModel>
				{
					Message = MessageResponseHelper.UpdateFailed("position"),
					IsSuccess = false,
					StatusCode = StatusCodes.Status500InternalServerError,
					RequestModel = request,
					ResponseModel = null
				};
			}
			catch (Exception e)
			{
				return new BaseModel<UpdatePositionResponseModel, UpdatePositionRequestModel>
				{
					Message = e.Message,
					IsSuccess = false,
					StatusCode = StatusCodes.Status500InternalServerError,
				};
			}
		}

	}
}
