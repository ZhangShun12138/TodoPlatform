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
        // 3. �����ʼ�
        try
        {
            await _UserService.SendVerificationCodeAsync(request.Email);
            return Ok(new { Success = true });
        }
        catch (Exception)
        {
            // ��¼��־
            return StatusCode(500, new { Error = "�ʼ�����ʧ��" });
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
            // ��¼��־
            return StatusCode(500, new { Error = "ע��ʧ��" });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> UserLogin([FromBody] UserLoginRequest request)
    {
        // 3. �����ʼ�
        try
        {
            // var result = await _UserService.UserRegisterAsync(request.Email, request.Captcha, request.Password);
            return Ok(new { Success = true });
        }
        catch (Exception)
        {
            // ��¼��־
            return StatusCode(500, new { Error = "ע��ʧ��" });
        }
    }
}
