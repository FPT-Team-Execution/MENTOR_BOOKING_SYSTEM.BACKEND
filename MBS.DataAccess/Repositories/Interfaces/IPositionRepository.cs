using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Interfaces
{
    public interface IPositionRepository : IBaseRepository<Position>
    {
        Task<Position> GetPositionByIdAsync(Guid id);
        Task<Pagination<Position>> GetPositions(int page, int size);
    }
}
