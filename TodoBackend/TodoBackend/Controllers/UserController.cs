using Microsoft.AspNetCore.Mvc;
using TodoBackend.Interfaces;
using TodoBackend.Requests;
using TodoBackend.tools;

namespace TodoBackend.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;
    private readonly JwtHelper _jwtHelper;
    private readonly JwtTokenSettings _jwtSettings;

    public UserController(
    ILogger<UserController> logger,
    IUserService userService,
    JwtHelper jwtHelper,
    JwtTokenSettings jwtSettings)
    {
        _logger = logger;
        _userService = userService;
        _jwtHelper = jwtHelper;
        _jwtSettings = jwtSettings;
    }

    [HttpPost("send-code")]
    public async Task<IActionResult> SendVerificationCode([FromBody] VerificationCodRequest request)
    {
        // 3. 发送邮件
        try
        {
            await _userService.SendVerificationCodeAsync(request.Email);
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
            var result = await _userService.UserRegisterAsync(request.Email, request.Captcha, request.Password);
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
        try
        {
            // 1. 验证用户（假设你有一个 UserService 来处理用户验证）
            var user = await _userService.AuthenticateUserAsync(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized(new { Error = "用户名或密码错误" });
            }

            // 2. 生成 JWT Token
            var token = _jwtHelper.GenerateToken(user.Username);

            // 3. 返回 Token 和成功信息
            return Ok(new
            {
                Success = true,
                Token = token,
                ExpiresIn = _jwtSettings.TokenExpires * 3600 // 过期时间（秒）
            });
        }
        catch (Exception ex)
        {
            // 记录日志
            _logger.LogError(ex, "登录失败");
            return StatusCode(500, new { Error = "登录失败，请稍后重试" });
        }
    }
}
