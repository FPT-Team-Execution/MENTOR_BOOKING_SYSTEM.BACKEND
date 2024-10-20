using Org.BouncyCastle.Pqc.Crypto.Lms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Persistents.Configurations.SeedData
{
    public class DbInitializer
    {
        private readonly MBSContext _mBSContext;
        public DbInitializer(MBSContext mBSContext)
        {
            _mBSContext = mBSContext;
        }

        public void Initialize<T>(List<T> entities) where T : class
        {
            try
            {
                bool hasDuplicates = entities.Any(entity => _mBSContext.Set<T>().Any(e => e.Equals(entity)));

                if (hasDuplicates)
                {
                    Console.WriteLine($"Skipping initialization of {typeof(T).Name} due to existing duplicates.");
                    return;
                }

                foreach (var entity in entities)
                {
                    _mBSContext.Set<T>().Add(entity);
                }

                _mBSContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while initializing {typeof(T).Name}: {ex.Message}");
            }
        }

    }
}
