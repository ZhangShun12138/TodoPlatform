using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoBackend.Models;

namespace TodoBackend.Data.Configurations;

public class TasksConfiguration : IEntityTypeConfiguration<Tasks>
{
    public void Configure(EntityTypeBuilder<Tasks> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        var order = 1;
        builder
            .Property(task => task.Id)
            .HasColumnOrder(order++)
            .IsRequired();
        builder
            .Property(task => task.Title)
            .HasColumnOrder(order++)
            .HasMaxLength(50)
            .IsRequired();
        builder
            .Property(task => task.Description)
            .HasColumnOrder(order++)
            .HasMaxLength(200)
            .IsRequired();
        builder
            .Property(task => task.IsCompleted)
            .HasColumnOrder(order++)
            .IsRequired();
        builder
            .Property(task => task.CreatedAt)
            .HasColumnOrder(order++)
            .IsRequired();
        builder
            .Property(task => task.DueDate)
            .HasColumnOrder(order++);
        builder
            .Property(task => task.Priority)
            .HasColumnOrder(order++)
            .IsRequired();
        builder
            .Property(task => task.UserId)
            .HasColumnOrder(order++)
            .HasMaxLength(50)
            .IsRequired();
        builder.HasIndex(x => new { x.UserId });
    }
}
