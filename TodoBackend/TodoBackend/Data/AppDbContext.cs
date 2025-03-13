using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;
using TodoBackend.Models;

namespace TodoBackend.Data;

// Data/AppDbContext.cs
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<VerificationCode> VerificationCodes { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<JsonElement>()
            .HaveConversion<JsonElementConverter>();
    }
}
