using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Majors;
using MBS.Application.Services.Interfaces;
using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;


namespace MBS.Application.Services.Implements
{
    public class MajorService : IMajorService
    {
        private IMajorRepository _majorRepository;
        public MajorService(IMajorRepository majorRepository)
        {
            _majorRepository = majorRepository;
        }

        //OK
        public async Task<BaseModel<GetAllMajorResponseModel, GetAllMajorReuqestModel>> GetAllMajor()
        {
            var result = await _majorRepository.GetAllAsync();
            if (result == null)
            {
                return new BaseModel<GetAllMajorResponseModel, GetAllMajorReuqestModel>()
                {
                    Message = MessageResponseHelper.Fail("Get all " + nameof(Major)),
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false
                };
            }

            return new BaseModel<GetAllMajorResponseModel, GetAllMajorReuqestModel>()
            {
                Message = MessageResponseHelper.Successfully("Get all " + nameof(Major)),
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = true,
                ResponseModel = new GetAllMajorResponseModel()
                {
                    majors = result.ToList()
                }
            };
        }
        //OK
        public async Task<BaseModel<GetMajorResponseModel, GetMajorRequestModel>> GetMajor(GetMajorRequestModel request)
        {
            var resultSet = await _majorRepository.GetAsync(i => i.Id == request.id);
            if (resultSet == null) 
            {
                return new BaseModel<GetMajorResponseModel, GetMajorRequestModel>()
                {
                    Message = MessageResponseHelper.Fail("Get " + nameof(Major)),
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false,
                    RequestModel = request
                };
            }
            return new BaseModel<GetMajorResponseModel, GetMajorRequestModel>()
            {
                Message = MessageResponseHelper.Successfully("Get " + nameof(Major)),
                StatusCode = StatusCodes.Status202Accepted,
                IsSuccess = true,
                ResponseModel = new GetMajorResponseModel()
                {
                    major = resultSet
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
            
            await _majorRepository.AddAsync(major);
            return new BaseModel<CreateMajorResponseModel, CreateMajorRequestModel>()
            {
                Message = MessageResponseHelper.Successfully("Created" + nameof(Major)),
                StatusCode = StatusCodes.Status202Accepted,
                IsSuccess = true,
                ResponseModel = new CreateMajorResponseModel()
                {
                    Major = major
                }
            };
        }


        public async Task<BaseModel<UpdateMajorResponseModel, UpdateMajorRequestModel>> UpdateMajor(UpdateMajorRequestModel request)
        {
            var majorSet = await _majorRepository.GetAsync(i => i.Id == request.id);
            if (majorSet == null)
            {
                return new BaseModel<UpdateMajorResponseModel, UpdateMajorRequestModel>()
                {
                    Message = MessageResponseHelper.Fail("Update " + nameof(Major)),
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false,
                    RequestModel = request
                };
            }
            majorSet.Name = request.Name;
            _majorRepository.UpdateAsync(majorSet);
            return new BaseModel<UpdateMajorResponseModel, UpdateMajorRequestModel>()
            {
                Message = MessageResponseHelper.Successfully("Update " + nameof(Major)),
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = true,
                ResponseModel = new UpdateMajorResponseModel()
                {
                    UpdatedMajor = majorSet
                }
            };
        }
        public async Task<BaseModel<RemoveMajorResponseModel, RemoveMajorRequestModel>> RemoveMajor(RemoveMajorRequestModel request)
        {
            var majorSet = await _majorRepository.GetAsync(i => i.Id == request.id);
            if (majorSet == null)
            {
                return new BaseModel<RemoveMajorResponseModel, RemoveMajorRequestModel>()
                {
                    Message = MessageResponseHelper.Fail("Remove " + nameof(Major)),
                    StatusCode = StatusCodes.Status400BadRequest,
                    IsSuccess = false,
                    RequestModel = request
                };
            }
            await _majorRepository.RemoveAsync(majorSet);
            return new BaseModel<RemoveMajorResponseModel, RemoveMajorRequestModel>()
            {
                Message = MessageResponseHelper.Successfully("Remove " + nameof(Major)),
                StatusCode = StatusCodes.Status200OK,
                IsSuccess = true,
                ResponseModel = new RemoveMajorResponseModel()
                {
                    success = "Deleted !!!"
                }
            };
            
        }

        
    }
}

