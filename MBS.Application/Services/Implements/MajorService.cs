using AutoMapper;
using MBS.Application.Helpers;
using MBS.Application.Models.General;
using MBS.Application.Models.Majors;
using MBS.Application.Models.Mentor;
using MBS.Application.Services.Interfaces;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO;
using MBS.DataAccess.Repositories;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Drawing;


namespace MBS.Application.Services.Implements
{
	public class MajorService : BaseService2<MajorService>, IMajorService
	{
		private readonly IMajorRepository _majorRepository;
		private readonly IMentorMajorRepository _mentorMajorRepository;

        //OK
        public async Task<BaseModel<Pagination<MajorResponseDTO>>> GetMajors(int page, int size)
        {
            var result = await _majorRepository.GetPagedListAsync(page: page, size: size);
            var MajorDTOList = new List<MajorResponseDTO>();
            foreach (var item in result.Items) 
            {
                var majorFound = await _majorRepository.GetMajorByIdAsync(item.Id);
                var majorDTO = new MajorResponseDTO
                {
                    Id = majorFound.Id,
                    Name = majorFound.Name,
                    ParentName = majorFound.ParentMajor?.Name,
                    CreatedOn = majorFound.CreatedOn,
                    UpdatedOn = majorFound.UpdatedOn
                };
                MajorDTOList.Add(majorDTO);

                
            }

            var paginatedMajor = new Pagination<MajorResponseDTO>()
            {
                Items = MajorDTOList,
                PageSize = size,
                PageIndex = page
            };
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
                ResponseRequestModel =paginatedMajor
            };
        }
        //OK
        public async Task<BaseModel<MajorModel>> GetMajorId(Guid requestId)
        {
            var resultSet = await _majorRepository.GetByIdAsync(requestId, "Id");
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
		public MajorService(
			IMajorRepository majorRepository,
			IMentorMajorRepository mentorMajorRepository,
			ILogger<MajorService> logger, IMapper mapper) : base(logger, mapper)
		{
			_majorRepository = majorRepository;
			_mentorMajorRepository = mentorMajorRepository;
		}

		//OK
		//public async Task<BaseModel<Pagination<MajorResponseDTO>>> GetMajors(int page, int size)
		//{
		//	var result = await _majorRepository.GetPagedListAsync(page: page, size: size);
		//	if (result == null)
		//	{
		//		return new BaseModel<Pagination<MajorResponseDTO>>()
		//		{
		//			Message = MessageResponseHelper.Fail("Get all " + nameof(Major)),
		//			StatusCode = StatusCodes.Status400BadRequest,
		//			IsSuccess = false
		//		};
		//	}

		//	return new BaseModel<Pagination<MajorResponseDTO>>()
		//	{
		//		Message = MessageResponseHelper.Successfully("Get all " + nameof(Major)),
		//		StatusCode = StatusCodes.Status200OK,
		//		IsSuccess = true,
		//		ResponseRequestModel = _mapper.Map<Pagination<MajorResponseDTO>>(result)
		//	};
		//}
		////OK
		//public async Task<BaseModel<MajorModel>> GetMajorId(Guid requestId)
		//{
		//	var resultSet = await _majorRepository.GetByIdAsync(requestId, "Id");
		//	if (resultSet == null)
		//	{
		//		return new BaseModel<MajorModel>()
		//		{
		//			Message = MessageResponseHelper.Fail("Get " + nameof(Major)),
		//			StatusCode = StatusCodes.Status404NotFound,
		//			IsSuccess = false,
		//		};
		//	}
		//	return new BaseModel<MajorModel>()
		//	{
		//		Message = MessageResponseHelper.Successfully("Get " + nameof(Major)),
		//		StatusCode = StatusCodes.Status202Accepted,
		//		IsSuccess = true,
		//		ResponseRequestModel = new MajorModel()
		//		{
		//			MajorResponse = _mapper.Map<MajorResponseDTO>(resultSet)
		//		}
		//	};
		//}

		public async Task<BaseModel<CreateMajorResponseModel, CreateMajorRequestModel>> CreateNewMajorAsync(CreateMajorRequestModel request)
		{
			Major major = new()
			{
				Id = Guid.NewGuid(),
				Name = request.Name,
				ParentId = request.ParentId,
			};
			if (major == null)
			{
				return new BaseModel<CreateMajorResponseModel, CreateMajorRequestModel>()
				{
					Message = MessageResponseHelper.Fail("Created" + nameof(Major)),
					StatusCode = StatusCodes.Status400BadRequest,
					IsSuccess = false,
					RequestModel = request,
				};
			}


			await _majorRepository.CreateAsync(major);

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
			//var majorSet = await _unitOfWork.GetRepository<Major>().SingleOrDefaultAsync(i => i.Id == id);
			var majorSet = await _majorRepository.GetByIdAsync(id, "Id");
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
			//_unitOfWork.GetRepository<Major>().UpdateAsync(majorSet);
			//await _unitOfWork.CommitAsync();
			_majorRepository.Update(majorSet);
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
			//var majorSet = await _unitOfWork.GetRepository<Major>().SingleOrDefaultAsync(i => i.Id == id);
			var majorSet = await _majorRepository.GetByIdAsync(id, "Id");
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
			//_unitOfWork.GetRepository<Major>().UpdateAsync(majorSet);
			//await _unitOfWork.CommitAsync();
			_majorRepository.Update(majorSet);
			return new BaseModel()
			{
				Message = MessageResponseHelper.Successfully("Remove " + nameof(Major)),
				StatusCode = StatusCodes.Status200OK,
				IsSuccess = true
			};

		}

		public async Task<BaseModel<Pagination<MajorResponseDTO>>> GetMentorMajors(GetMentorMajorsRequest request)
		{
			var mentorMajors = await _mentorMajorRepository.GetMentorMajorsAsync(request.MentorId, request.Page, request.Size);

			if (mentorMajors == null)
			{
				return new BaseModel<Pagination<MajorResponseDTO>>()
				{
					IsSuccess = false,
					Message = MessageResponseHelper.Empty("Major")
				};
			}

			List<Major?> majors = new List<Major?>();

			foreach (var mentorMajor in mentorMajors.Items)
			{
				majors.Add(mentorMajor.Major);
			}

			var paginationMajors = new Pagination<Major>()
			{
				Items = majors,
				PageIndex = request.Page,
				PageSize = request.Size,
				TotalPages = majors.Count,
			};

			return new BaseModel<Pagination<MajorResponseDTO>>()
			{
				Message = MessageResponseHelper.Successfully("Get all " + nameof(Major)),
				StatusCode = StatusCodes.Status200OK,
				IsSuccess = true,
				ResponseRequestModel = _mapper.Map<Pagination<MajorResponseDTO>>(paginationMajors),
			};
		}

	}

}

