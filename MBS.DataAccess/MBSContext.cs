using MBS.Core.Common;
using MBS.Core.Entities;
using MBS.Shared.Services.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using MBS.Shared.Services.Interfaces;


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
            //seed data by builder
            MBSContextSeed.SeedPositionAsync(builder);
            base.OnModelCreating(builder);
        }

        public DbSet<Mentor> Mentors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<CalendarEvent> CalendarEvents { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<MeetingMember> MeetingMembers { get; set; }
        public DbSet<MentorMajor> MentorMajors { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Progress> Progresses { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<PointTransaction> PointTransactions { get; set; }
 
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
