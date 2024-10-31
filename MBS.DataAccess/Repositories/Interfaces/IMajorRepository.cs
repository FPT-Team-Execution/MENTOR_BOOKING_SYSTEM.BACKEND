using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Interfaces
{
    public interface IMajorRepository : IBaseRepository<Major>
    {
        Task<Major> GetMajorByIdAsync(Guid majorId);
        Task<Pagination<Major>> GetPagedListBaseAsync(int page, int size);

    }

}
