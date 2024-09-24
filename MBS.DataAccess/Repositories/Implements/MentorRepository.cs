using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Implements
{
    public class MentorRepository : BaseRepository<Mentor>, IMentorRepository
    {
        private readonly MBSContext _context;
        public MentorRepository(MBSContext context) : base(context)
        {
            _context = context;
        }
    }
}
