using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Persistents.Configurations
{
    public class DatabaseConfig
    {
        public bool UseInMemoryDatabase { get; set; }

        public string ConnectionString { get; set; }
    }
}
