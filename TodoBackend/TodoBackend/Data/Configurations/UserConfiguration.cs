using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoBackend.Models;

namespace TodoBackend.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var order = 1;

        builder
            .Property(user => user.Id)
            .HasColumnOrder(order++)
            .IsRequired();

        builder
            .Property(user => user.Username)
            .HasColumnOrder(order++)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(user => user.PasswordHash)
            .HasColumnOrder(order++)
            .IsRequired();

        builder
            .Property(user => user.PasswordSalt)
            .HasColumnOrder(order++)
            .IsRequired();

        builder
            .Property(user => user.CreatedAt)
            .HasColumnOrder(order++)
            .IsRequired();

        builder.HasIndex(x => new { x.Username }).IsUnique().HasFilter(null);
    }
}
