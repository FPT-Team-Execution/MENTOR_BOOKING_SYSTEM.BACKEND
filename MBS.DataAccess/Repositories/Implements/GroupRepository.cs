<<<<<<< HEAD
﻿using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Implements
{
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        public GroupRepository(IBaseDAO<Group> dao) : base(dao)
        {
        }

        public Task<Group> GetGroupByIdAsync(Guid id)
        {
            return _dao.SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
=======
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MBS.DataAccess.Repositories.Implements;

public class GroupRepository(IBaseDAO<Group> dao) : BaseRepository<Group>(dao), IGroupRepository
{
    public async Task<Pagination<Group>> GetGroupsByStudentId(string studentId, int page, int size, string sortOrder)
    {
        return await _dao.GetPagingListAsync(
            predicate: g => g.StudentId == studentId,
            include: p => p.Include(x => x.Project),
            orderBy: p => (sortOrder.ToLower() == "asc") ? p.OrderBy(x => x.Project.CreatedOn) : p.OrderByDescending(x => x.Project.CreatedOn),
            page: page,
            size: size
        );
    }

}
>>>>>>> develop
