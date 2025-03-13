using System.ComponentModel.DataAnnotations;

namespace TodoBackend.Requests;

public class UserLoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    [RegularExpression(@"^.{8,20}$", ErrorMessage = "密码长度必须在 8 到 20 位之间。")]
    public string Password { get; set; } = default!;
}