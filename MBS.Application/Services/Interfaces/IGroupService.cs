using MBS.Application.Models.General;
using MBS.Application.Models.Groups;

namespace MBS.Application.Services.Interfaces
{
    public interface IGroupService
    {
        Task<BaseModel<CreateNewGroupResponseModel, CreateNewGroupRequestModel>> CreateNewGroupAsync(
        CreateNewGroupRequestModel request);

        Task<BaseModel<GetGroupResponseModel, GetGroupRequestModel>> GetGroup(
            GetGroupRequestModel request);

        Task<BaseModel<UpdateGroupResponseModel, UpdateGroupRequestModel>> UpdateGroup(UpdateGroupRequestModel request);

        Task<BaseModel<RemoveGroupResponseModel, RemoveGroupRequestModel>> RemoveGroup(
            RemoveGroupRequestModel request);
        Task<BaseModel<GetAllGroupResponseModel, GetAllGroupRequestModel>> GetAllGroup();
    }
}
