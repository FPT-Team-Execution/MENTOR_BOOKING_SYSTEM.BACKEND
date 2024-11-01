﻿using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Positions;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;

using MBS.DataAccess.Repositories;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MBS.DataAccess.DAO;


namespace MBS.Application.Services.Implements
{
    public class PositionService : BaseService2<PositionService>, IPositionService
    {
        private readonly IPositionRepository _positionRepository;

        public PositionService(
            IPositionRepository positionRepository,
            ILogger<PositionService> logger, IMapper mapper)
            : base(logger, mapper)
        {
            _positionRepository = positionRepository;
        }

        public async Task<BaseModel<CreatePositionResponseModel, CreatePositionRequestModel>> CreateNewPosition(CreatePositionRequestModel request)
        {
            Position position = new()
            {
                Id = Guid.NewGuid(),
                Name = request.name,
                Description = request.description,
            };

            var addRs = await _positionRepository.CreateAsync(position);
            if (!addRs)
            {
                return new BaseModel<CreatePositionResponseModel, CreatePositionRequestModel>()
                {
                    Message = MessageResponseHelper.Fail("Created " + nameof(Position)),
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false,
                    RequestModel = request,
                };
            }


            return new BaseModel<CreatePositionResponseModel, CreatePositionRequestModel>()
            {
                Message = MessageResponseHelper.Successfully("Created " + nameof(Position)),
                StatusCode = StatusCodes.Status202Accepted,
                IsSuccess = true,
                ResponseModel = new CreatePositionResponseModel()
                {
                    Id = position.Id,
                }
            };
        }

        public async Task<BaseModel<PositionModel>> GetPositionId(Guid requestId)
        {
            var position = await _positionRepository.GetByIdAsync(requestId, "Id");
            if (position == null)
            {
                return new BaseModel<PositionModel>()
                {
                    Message = MessageResponseHelper.Fail("Get " + nameof(Position)),
                    StatusCode = StatusCodes.Status404NotFound,
                    IsSuccess = false,
                };
            }

            return new BaseModel<PositionModel>()
            {
                Message = MessageResponseHelper.Successfully("Get " + nameof(Position)),
                StatusCode = StatusCodes.Status202Accepted,
                IsSuccess = true,
                ResponseRequestModel = new PositionModel()
                {
                    positionResponse = _mapper.Map<PositionResponseDTO>(position)
                }
            };
        }

        public async Task<BaseModel<PositionModel>> UpdatePosition(Guid id, UpdatePositionRequestModel request)
        {
            var position = await _positionRepository.GetByIdAsync(id, "Id");
            if (position == null)
            {
                return new BaseModel<PositionModel>()
                {
                    Message = MessageResponseHelper.Fail("Update " + nameof(Position)),
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false,
                };
            }

            position.Name = request.name;
            position.Description = request.description;

            var updateRs = _positionRepository.Update(position);
            if(!updateRs)
                return new BaseModel<PositionModel>()
                {
                    Message = MessageResponseHelper.UpdateFailed("position"),
                    StatusCode = StatusCodes.Status200OK,
                    IsSuccess = false
                };

            return new BaseModel<PositionModel>()
            {
                Message = MessageResponseHelper.Successfully("Update " + nameof(Position)),
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = true,
                ResponseRequestModel = new PositionModel()
                {
                    positionResponse = _mapper.Map<PositionResponseDTO>(position)
                }
            };
        }

        public async Task<BaseModel> RemovePosition(Guid id)
        {
            var position = await _positionRepository.GetByIdAsync(id, "Id");
            if (position == null)
            {
                return new BaseModel()
                {
                    Message = MessageResponseHelper.Fail("Remove " + nameof(Position)),
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false,
                };
            }

            position.Status = Core.Enums.StatusEnum.Deactivated;
            var updateRs = _positionRepository.Update(position);
            if(!updateRs)
                return new BaseModel()
                {
                    Message = MessageResponseHelper.UpdateFailed("position"),
                    StatusCode = StatusCodes.Status200OK,
                    IsSuccess = false
                };

            return new BaseModel()
            {
                Message = MessageResponseHelper.Successfully("Remove " + nameof(Position)),
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = true
            };
        }

        public async Task<BaseModel<Pagination<PositionResponseDTO>>> GetPositions(int page, int size)
        {
            var result = await _positionRepository.GetPagedListAsync(page: page, size: size);
            var positionDTOList = new List<PositionResponseDTO>();

            foreach (var item in result.Items)
            {
                var positionFound = await _positionRepository.GetPositionByIdAsync(item.Id);
                var positionDTO = new PositionResponseDTO
                {
                    Name = positionFound.Name,
                    Description = positionFound.Description,
                };
                positionDTOList.Add(positionDTO);
            }

            var paginatedPosition = new Pagination<PositionResponseDTO>
            {
                Items = positionDTOList,
                PageSize = size,
                PageIndex = page
            };

            if (result == null)
            {
                return new BaseModel<Pagination<PositionResponseDTO>>
                {
                    Message = MessageResponseHelper.Fail("Get all " + nameof(Position)),
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false
                };
            }

            return new BaseModel<Pagination<PositionResponseDTO>>
            {
                Message = MessageResponseHelper.Successfully("Get all " + nameof(Position)),
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = true,
                ResponseRequestModel = paginatedPosition
            };
        }

    }
}
