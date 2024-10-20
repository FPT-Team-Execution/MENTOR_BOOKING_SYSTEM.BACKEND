using MBS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System;
using System.Collections.Generic;

namespace MBS.DataAccess.Persistents.Configurations.SeedData
{
    public class SeedMajors
    {
        private readonly MBSContext  mBSContext;
        public SeedMajors(MBSContext dbInitialize)
        {
            mBSContext = dbInitialize;
        }

        public void SeedingMajors()
        {
            var defaultMajors = new List<Major>
            {
                new Major { Id = Guid.Parse("903b6085-4cc3-47f3-bbdd-0f8319e5aabb"), Name = "SE", Status = Core.Enums.StatusEnum.Activated },
                new Major { Id = Guid.Parse("71577eaf-ebf1-4b23-a48d-cf8561b1c7db"), Name = "SS", Status = Core.Enums.StatusEnum.Activated },
                new Major { Id = Guid.Parse("dfdb83a4-18e0-447e-9ec8-7c8b39ee6f3a"), Name = "SA", Status = Core.Enums.StatusEnum.Activated }
            };
            Initialize(defaultMajors);
        }


        public void Initialize<T>(List<T> entities) where T : class
        {
            try
            {
                foreach (var entity in entities)
                {
                    if (!mBSContext.Set<T>().Any(e => e.Equals(entity)))
                    {
                        mBSContext.Set<T>().Add(entity);
                    }
                }
                mBSContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while initializing {typeof(T).Name}: {ex.Message}");
            }
        }

    }
}
