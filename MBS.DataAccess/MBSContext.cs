using MBS.Core.Common;
using MBS.Core.Entities;
using MBS.Core.Enums;
using MBS.Shared.Services;
using MBS.Shared.Services.Implements;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess
{
    public class MBSContext : IdentityDbContext
    {
        private readonly IClaimService _claimService;
        public MBSContext(DbContextOptions<MBSContext> options, IClaimService claimService) : base(options)
        {
            _claimService = claimService;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        public DbSet<Mentor> Mentors { get; set; }

        public DbSet<Student> Students { get; set; }

        public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<IAuditedEntity>())
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _claimService.GetUserId();
                        entry.Entity.CreatedOn = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = _claimService.GetUserId();
                        entry.Entity.UpdatedOn = DateTime.Now;
                        break;
                }

            return await base.SaveChangesAsync(cancellationToken);
        }
       
    }
}
