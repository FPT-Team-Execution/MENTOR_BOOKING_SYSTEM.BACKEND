using MBS.Application.Models.General;
using MBS.Application.Models.Majors;

namespace MBS.Application.Services.Interfaces
{
    public interface IMajorService
    {
        Task<BaseModel<CreateMajorResponseModel, CreateMajorRequestModel>> CreateNewMajorAsync(
        CreateMajorRequestModel request);

        Task<BaseModel<GetMajorResponseModel, GetMajorRequestModel>> GetMajor(
            GetMajorRequestModel request);

        Task<BaseModel<UpdateMajorResponseModel, UpdateMajorRequestModel>> UpdateMajor(UpdateMajorRequestModel request);

        Task<BaseModel<RemoveMajorResponseModel, RemoveMajorRequestModel>> RemoveMajor(
            RemoveMajorRequestModel request);
        Task<BaseModel<GetAllMajorResponseModel, GetAllMajorReuqestModel>> GetAllMajor();

    }
}
