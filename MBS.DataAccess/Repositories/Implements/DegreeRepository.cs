using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories.Implements;

public class DegreeRepository : BaseRepository<Degree>, IDegreeRepository
{
    private readonly MBSContext _context;

    public DegreeRepository(MBSContext context) : base(context)
    {
        _context = context;
    }
}