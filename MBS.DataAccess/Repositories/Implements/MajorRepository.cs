using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Implements
{
	public class MajorRepository : BaseRepository<Major>, IMajorRepository
    {
        public MajorRepository(IBaseDAO<Major> dao) : base(dao)
        {
        }

        public async Task<Major> GetMajorByIdAsync(Guid majorId)
        {
            return await _dao.SingleOrDefaultAsync(x => x.Id == majorId);   
        }
	}
}



