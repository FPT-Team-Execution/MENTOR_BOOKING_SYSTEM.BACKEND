using MBS.Core.Entities;
using MBS.DataAccess.Repositories.Interfaces;

namespace MBS.DataAccess.Repositories.Implements;

public class FeedbackRepository(MBSContext context) : BaseRepository<Feedback>(context), IFeedbackRepository;   