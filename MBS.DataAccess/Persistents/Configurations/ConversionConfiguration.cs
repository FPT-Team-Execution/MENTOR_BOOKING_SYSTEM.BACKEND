using MBS.Core.Entities;
using MBS.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Persistents.Configurations
{
    public class MeetingConfiguration : IEntityTypeConfiguration<Meeting>
    {
        public void Configure(EntityTypeBuilder<Meeting> builder)
        {
            builder.Property(e => e.Status)
            .HasConversion(
                v => v.ToString(),
                v => (MeetingStatusEnum)Enum.Parse(typeof(MeetingStatusEnum), v)
            );
        }
    }
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.Property(e => e.Status)
            .HasConversion(
                v => v.ToString(),
                v => (ProjectStatusEnum)Enum.Parse(typeof(ProjectStatusEnum), v)
            );
        }
    }
    public class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.Property(e => e.Status)
            .HasConversion(
                v => v.ToString(),
                v => (RequestStatusEnum)Enum.Parse(typeof(RequestStatusEnum), v)
            );
        }
    }
    public class MajorConfiguration : IEntityTypeConfiguration<Major>
    {
        public void Configure(EntityTypeBuilder<Major> builder)
        {
            builder.Property(e => e.Status)
            .HasConversion(
                v => v.ToString(),
                v => (StatusEnum)Enum.Parse(typeof(StatusEnum), v)
            );
        }
    }
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.Property(e => e.Status)
            .HasConversion(
                v => v.ToString(),
                v => (StatusEnum)Enum.Parse(typeof(StatusEnum), v)
            );
        }
    }
    public class CalendarEventConfiguration : IEntityTypeConfiguration<CalendarEvent>
    {
        public void Configure(EntityTypeBuilder<CalendarEvent> builder)
        {
            builder.Property(e => e.Status)
            .HasConversion(
                v => v.ToString(),
                v => (StatusEnum)Enum.Parse(typeof(StatusEnum), v)
            );
        }
    }
}
