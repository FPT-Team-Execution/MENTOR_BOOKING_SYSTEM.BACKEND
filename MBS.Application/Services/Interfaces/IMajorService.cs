using MBS.Application.Models.General;
using MBS.Application.Models.Majors;
using MBS.Application.Models.Mentor;
using MBS.Application.Models.User;
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Application.Services.Interfaces
{
	public interface IMajorService
	{
		Task<BaseModel<CreateMajorResponseModel, CreateMajorRequestModel>> CreateNewMajorAsync(
		CreateMajorRequestModel request);

		Task<BaseModel<MajorModel>> GetMajorId(Guid requestId);


		Task<BaseModel<MajorModel>> UpdateMajor(Guid id, UpdateMajorRequestModel request);


		Task<BaseModel> RemoveMajor(Guid id);


		Task<BaseModel<Pagination<MajorResponseDTO>>> GetMajors(int page, int size);
		Task<BaseModel<Pagination<GetMentorMajorsResponse>>> GetMentorMajors(GetMentorMajorsRequest request);

	}
}
