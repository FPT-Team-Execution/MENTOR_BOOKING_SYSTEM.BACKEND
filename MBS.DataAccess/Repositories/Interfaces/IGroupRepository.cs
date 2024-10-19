<<<<<<< HEAD
﻿using MBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Interfaces
{
    public interface IGroupRepository : IBaseRepository<Group>
    {
        Task<Group> GetGroupByIdAsync(Guid id);
    }
}
=======
using MBS.Core.Common.Pagination;
using MBS.Core.Entities;

namespace MBS.DataAccess.Repositories.Interfaces;

public interface IGroupRepository : IBaseRepository<Group>
{
    Task<Pagination<Group>> GetGroupsByStudentId(string studentId, int page, int size, string sortOrder);
}
>>>>>>> develop
