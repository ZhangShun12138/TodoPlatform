namespace TodoBackend.Models;

public class VerificationCode
{
    public int Id { get; set; }
    public string Email { get; set; } = default!;
    public string Code { get; set; } = default!;
    public DateTimeOffset ExpireTime { get; set; } = DateTimeOffset.Now.AddMinutes(5); // 5分钟有效期
    public bool IsUsed { get; set; } = false;
}