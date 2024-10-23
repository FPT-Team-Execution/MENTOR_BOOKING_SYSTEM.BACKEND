using MBS.Application.Models.General;
using MBS.Application.Models.Groups;
using MBS.Core.Common.Pagination;
using System;
using System.Threading.Tasks;

namespace MBS.Application.Services.Interfaces
{
    public interface IGroupService
    {
        Task<BaseModel<CreateNewGroupResponseModel, CreateNewGroupRequestModel>> CreateNewGroupAsync(CreateNewGroupRequestModel request);

        Task<BaseModel<GroupModel>> GetGroupId(Guid requestId);

        Task<BaseModel<GroupModel>> UpdateGroup(Guid id, UpdateGroupRequestModel request);

        Task<BaseModel> RemoveGroup(Guid id);

        Task<BaseModel<Pagination<GroupResponseDTO>>> GetGroups(int page, int size);
        Task<BaseModel<GroupStudentsResponseDTO>> GetStudentsInGroupByProjectId(Guid projectId);
        Task<BaseModel<List<StudentSearchDTO>>> SearchStudent(string searchItem);

    }
}
