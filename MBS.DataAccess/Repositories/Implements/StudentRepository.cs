using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Repositories.Implements
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        private readonly MBSContext _context;

        public StudentRepository(MBSContext context) : base(context)
        {
            _context = context;
        }
    }
}
