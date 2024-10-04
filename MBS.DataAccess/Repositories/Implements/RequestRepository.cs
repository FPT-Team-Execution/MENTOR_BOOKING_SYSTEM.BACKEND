using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories.Implements;

public class RequestRepository(MBSContext context) : BaseRepository<Request>(context), IRequestRepository;