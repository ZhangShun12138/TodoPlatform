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
        // 3. �����ʼ�
        try
        {
            await _userService.SendVerificationCodeAsync(request.Email);
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
            var result = await _userService.UserRegisterAsync(request.Email, request.Captcha, request.Password);
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
        try
        {
            // 1. ��֤�û�����������һ�� UserService �������û���֤��
            var user = await _userService.AuthenticateUserAsync(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized(new { Error = "�û������������" });
            }

            // 2. ���� JWT Token
            var token = _jwtHelper.GenerateToken(user.Username);

            // 3. ���� Token �ͳɹ���Ϣ
            return Ok(new
            {
                Success = true,
                Token = token,
                ExpiresIn = _jwtSettings.TokenExpires * 3600 // ����ʱ�䣨�룩
            });
        }
        catch (Exception ex)
        {
            // ��¼��־
            _logger.LogError(ex, "��¼ʧ��");
            return StatusCode(500, new { Error = "��¼ʧ�ܣ����Ժ�����" });
        }
    }
}
