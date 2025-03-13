using System.ComponentModel.DataAnnotations;

namespace TodoBackend.Models;

// Models/User.cs
public class User
{
    public int Id { get; set; }

    [EmailAddress]
    public string Username { get; set; } = default!;
    public byte[] PasswordHash { get; set; } = default!;
    public byte[] PasswordSalt { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
