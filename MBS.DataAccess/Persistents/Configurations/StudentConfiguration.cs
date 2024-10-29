using MBS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.DataAccess.Persistents.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(e => e.UserId);
            builder.HasOne(e => e.User)
                   .WithMany()
                   .HasForeignKey(e => e.UserId);

            builder.Property(ti => ti.University)
                   .HasMaxLength(100);

            builder.Property(ti => ti.WalletPoint)
                    .HasDefaultValue(0)
                    .IsRequired();

        }
    }
}
