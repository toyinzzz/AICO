using AICO.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AICO.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .HasMaxLength(256)
                .IsRequired();

            // Add other User entity configurations here if needed
            // For example, for Username, PasswordHash, etc.

            builder.Property(u => u.FirstName).HasMaxLength(100);
            builder.Property(u => u.LastName).HasMaxLength(100);
            builder.Property(u => u.Role).HasMaxLength(50);

            // BaseEntity properties
            builder.Property(u => u.CreatedAt).IsRequired();
            builder.Property(u => u.ModifiedAt).IsRequired();
            builder.Property(u => u.RowVersion).IsRowVersion();
        }
    }
}