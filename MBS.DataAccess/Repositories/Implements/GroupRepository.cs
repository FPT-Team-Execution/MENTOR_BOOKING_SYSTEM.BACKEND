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

        public Task<Group> GetGroupByProjectAndStudentIdAsync(Guid projectId, string studentId)
        {
            return _dao.SingleOrDefaultAsync(x => x.StudentId == studentId && x.ProjectId == projectId);
        }

        public async Task<IEnumerable<Group>> GetGroupsByProjectIdAsync(Guid project)
        {
            return await _dao.GetListAsync(a => a.ProjectId == project);
        }
    }
}
