using System.ComponentModel.DataAnnotations;

namespace TodoBackend.Requests;

public class UserRegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "验证码必须是 6 位数字。")]
    public string Captcha { get; set; } = default!;

    [Required]
    [RegularExpression(@"^.{8,20}$", ErrorMessage = "密码长度必须在 8 到 20 位之间。")]
    public string Password { get; set; } = default!;
}