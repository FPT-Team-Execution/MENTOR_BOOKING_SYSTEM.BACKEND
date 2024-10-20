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
    public class PointTransactionRepository : BaseRepository<PointTransaction>, IPointTransactionRepository
    {
        public PointTransactionRepository(IBaseDAO<PointTransaction> dao) : base(dao)
        {
        }
    }
}
