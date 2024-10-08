using MBS.Application.Models.General;
using MBS.Application.Models.Majors;
using MBS.Application.Models.User;
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

        Task<BaseModel<GetMajorResponseModel, GetMajorRequestModel>> GetMajor(
            GetMajorRequestModel request);

        Task<BaseModel<UpdateMajorResponseModel, UpdateMajorRequestModel>> UpdateMajor(Guid id, UpdateMajorRequestModel request);

        Task<BaseModel<RemoveMajorResponseModel, RemoveMajorRequestModel>> RemoveMajor(
            RemoveMajorRequestModel request);
        Task<BaseModel<GetAllMajorResponseModel, GetAllMajorReuqestModel>> GetAllMajor();

    }
}
