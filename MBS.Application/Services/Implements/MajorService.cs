using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Majors;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.Repositories;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace MBS.Application.Services.Implements
{
    public class MajorService : BaseService<MajorService>, IMajorService
    {
        public MajorService(IUnitOfWork unitOfWork, ILogger<MajorService> logger, IMapper mapper) : base(unitOfWork, logger, mapper)
        {
        }

		//OK
		public async Task<BaseModel<Pagination<MajorResponseDTO>>> GetMajors(int page, int size)
        {
            var result = await _unitOfWork.GetRepository<Major>().GetPagingListAsync(page: page, size: size);
            if (result == null)
            {
                return new BaseModel<Pagination<MajorResponseDTO>>()
                {
                    Message = MessageResponseHelper.Fail("Get all " + nameof(Major)),
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false
                };
            }

            return new BaseModel<Pagination<MajorResponseDTO>>()
            {
                Message = MessageResponseHelper.Successfully("Get all " + nameof(Major)),
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = true,
                ResponseRequestModel = _mapper.Map<Pagination<MajorResponseDTO>>(result)
            };
        }
        //OK
        public async Task<BaseModel<MajorModel>> GetMajorId(Guid requestId)
        {
            var resultSet = await _unitOfWork.GetRepository<Major>().SingleOrDefaultAsync(i => i.Id == requestId);
            if (resultSet == null) 
            {
                return new BaseModel<MajorModel>()
                {
                    Message = MessageResponseHelper.Fail("Get " + nameof(Major)),
                    StatusCode = StatusCodes.Status404NotFound,
                    IsSuccess = false,
                };
            }
            return new BaseModel<MajorModel>()
            {
                Message = MessageResponseHelper.Successfully("Get " + nameof(Major)),
                StatusCode = StatusCodes.Status202Accepted,
                IsSuccess = true,
                ResponseRequestModel = new MajorModel()
                {
                    MajorResponse = _mapper.Map<MajorResponseDTO>(resultSet)
                }
            };
        }

        public async Task<BaseModel<CreateMajorResponseModel, CreateMajorRequestModel>> CreateNewMajorAsync(CreateMajorRequestModel request)
        {
            Major major = new()
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                ParentId = request.ParentId,
            }; 
            if(major == null)
            {
                return new BaseModel<CreateMajorResponseModel, CreateMajorRequestModel>()
                {
                    Message = MessageResponseHelper.Fail("Created" + nameof(Major)),
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false,
                    RequestModel = request,
                };
            }
            
            await _unitOfWork.GetRepository<Major>().InsertAsync(major);
            await _unitOfWork.CommitAsync();
            return new BaseModel<CreateMajorResponseModel, CreateMajorRequestModel>()
            {
                Message = MessageResponseHelper.Successfully("Created" + nameof(Major)),
                StatusCode = StatusCodes.Status202Accepted,
                IsSuccess = true,
                ResponseModel = new CreateMajorResponseModel()
                {
                    MajorId = major.Id,
                }
            };
        }


        public async Task<BaseModel<MajorModel>> UpdateMajor(Guid id, UpdateMajorRequestModel request)
        {
            var majorSet = await _unitOfWork.GetRepository<Major>().SingleOrDefaultAsync(i => i.Id == id);
            if (majorSet == null)
            {
                return new BaseModel<MajorModel>()
                {
                    Message = MessageResponseHelper.Fail("Update " + nameof(Major)),
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false,
                };
            }
            majorSet.Name = request.Name;
            majorSet.ParentId = request.ParentId;
            _unitOfWork.GetRepository<Major>().UpdateAsync(majorSet);
            await _unitOfWork.CommitAsync();
            return new BaseModel<MajorModel>()
            {
                Message = MessageResponseHelper.Successfully("Update " + nameof(Major)),
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = true,
                ResponseRequestModel = new MajorModel()
                {
                    MajorResponse = _mapper.Map<MajorResponseDTO>(majorSet)
                }
            };
        }
        public async Task<BaseModel> RemoveMajor(Guid id)
        {
            var majorSet = await _unitOfWork.GetRepository<Major>().SingleOrDefaultAsync(i => i.Id == id);
            if (majorSet == null)
            {
                return new BaseModel()
                {
                    Message = MessageResponseHelper.Fail("Remove " + nameof(Major)),
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false,
                };
            }
            majorSet.Status = Core.Enums.StatusEnum.Deactivated;
            _unitOfWork.GetRepository<Major>().UpdateAsync(majorSet);
            await _unitOfWork.CommitAsync();
            return new BaseModel()
            {
                Message = MessageResponseHelper.Successfully("Remove " + nameof(Major)),
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = true
            };
            
        }

        
    }
}

