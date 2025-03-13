using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoBackend.Models;

namespace TodoBackend.Data.Configurations;

public class VerificationCodeConfiguration : IEntityTypeConfiguration<VerificationCode>
{
    public void Configure(EntityTypeBuilder<VerificationCode> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var order = 1;
        builder
            .Property(verificationCode => verificationCode.Id)
            .HasColumnOrder(order++)
            .IsRequired();

        builder
            .Property(verificationCode => verificationCode.Email)
            .HasColumnOrder(order++)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .Property(verificationCode => verificationCode.Code)
            .HasColumnOrder(order++)
            .HasMaxLength(6)
            .IsFixedLength()
            .IsRequired();

        builder
            .Property(verificationCode => verificationCode.ExpireTime)
            .HasColumnOrder(order++)
            .IsRequired();

        builder
            .Property(verificationCode => verificationCode.IsUsed)
            .HasColumnOrder(order++)
            .IsRequired();

        builder
            .HasIndex(x => new { x.Email, x.Code, x.ExpireTime }).IsUnique().HasFilter(null);
    }
}
