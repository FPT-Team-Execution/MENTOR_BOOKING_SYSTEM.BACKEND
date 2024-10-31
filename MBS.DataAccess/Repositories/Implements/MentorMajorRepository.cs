using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using MBS.DataAccess.DAO.Interfaces;
using MBS.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Implements
{
	public class MentorMajorRepository : BaseRepository<MentorMajor>, IMentorMajorRepository
	{

		public MentorMajorRepository(IBaseDAO<MentorMajor> dao) : base(dao)
		{
		}

		public async Task<Pagination<MentorMajor>> GetMentorMajorsAsync(string mentorId, int page, int size)
		{
			return await _dao.GetPagingListAsync(predicate: x => x.MentorId == mentorId, page: page, size: size, include: x => x.Include(x => x.Major));
		}
	}
}
