using MBS.Core.Common.Pagination;
using MBS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Interfaces
{
	public interface IMentorMajorRepository : IBaseRepository<MentorMajor>
	{
		Task<Pagination<MentorMajor>> GetMentorMajorsAsync(string mentorId, int page, int size);
	}
}
