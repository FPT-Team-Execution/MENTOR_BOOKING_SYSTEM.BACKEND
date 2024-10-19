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

            List<Major> defaultMajor =
                [
                new Major
                {
                    Id = Guid.Parse("903b6085-4cc3-47f3-bbdd-0f8319e5aabb"),
                    Name = "SE",
                    Status = StatusEnum.Activated
                },
                new Major
                {
                    Id = Guid.Parse("71577eaf-ebf1-4b23-a48d-cf8561b1c7db"),
                    Name = "SS",
                    Status = StatusEnum.Activated
                },
                new Major
                {
                    Id = Guid.Parse("dfdb83a4-18e0-447e-9ec8-7c8b39ee6f3a"),
                    Name = "SA",
                    Status = StatusEnum.Activated
                }
                ];
            builder.HasData(defaultMajor);
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
                v => (EventStatus)Enum.Parse(typeof(EventStatus), v)
            );
        }
    }
    public class PointTransactionConfiguration : IEntityTypeConfiguration<PointTransaction>
    {
        public void Configure(EntityTypeBuilder<PointTransaction> builder)
        {
            builder.Property(e => e.TransactionType)
            .HasConversion(
                v => v.ToString(),
                v => (TransactionTypeEnum)Enum.Parse(typeof(TransactionTypeEnum), v)
            );
            builder.Property(e => e.Currency)
            .HasConversion(
                v => v.ToString(),
                v => (PointCurrencyEnum)Enum.Parse(typeof(PointCurrencyEnum), v)
            );
			builder.Property(e => e.Status)
			.HasConversion(
				v => v.ToString(),
				v => (TransactionStatusEnum)Enum.Parse(typeof(TransactionStatusEnum), v)
			);
			builder.Property(e => e.Kind)
			.HasConversion(
				v => v.ToString(),
				v => (TransactionKindEnum)Enum.Parse(typeof(TransactionKindEnum), v)
			);
		}
    }
}
