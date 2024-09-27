using MBS.Core.Entities;
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
        private readonly MBSContext _context;
        public MajorRepository(MBSContext context) : base(context)
        {
            _context = context;
        }

        public Task UpdateAsync(Task<Major?>? major)
        {
            throw new NotImplementedException();
        }
    }
}
