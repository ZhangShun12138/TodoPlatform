using Microsoft.AspNetCore.Mvc;
using TodoBackend.Interfaces;
using TodoBackend.Requests;

namespace TodoBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _UserService;

    public UserController(
        ILogger<UserController> logger,
        IUserService UserService)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(UserService);

        _logger = logger;
        _UserService = UserService;
    }

    [HttpPost("send-code")]
    public async Task<IActionResult> SendVerificationCode([FromBody] VerificationCodRequest request)
    {
        // 3. 发送邮件
        try
        {
            await _UserService.SendVerificationCodeAsync(request.Email);
            return Ok(new { Success = true });
        }
        catch (Exception)
        {
            // 记录日志
            return StatusCode(500, new { Error = "邮件发送失败" });
        }
    }

    [HttpPost("user-register")]
    public async Task<IActionResult> UserRegister([FromBody] UserRegisterRequest request)
    {
        try
        {
            var result = await _UserService.UserRegisterAsync(request.Email, request.Captcha, request.Password);
            return Ok(new { Success = true, Result = result });
        }
        catch (Exception)
        {
            // 记录日志
            return StatusCode(500, new { Error = "注册失败" });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> UserLogin([FromBody] UserLoginRequest request)
    {
        // 3. 发送邮件
        try
        {
            // var result = await _UserService.UserRegisterAsync(request.Email, request.Captcha, request.Password);
            return Ok(new { Success = true });
        }
        catch (Exception)
        {
            // 记录日志
            return StatusCode(500, new { Error = "注册失败" });
        }
    }
}
