using ClassLibrary;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MimeKit;
using TodoBackend.Data;
using TodoBackend.Interfaces;
using TodoBackend.Models;

namespace TodoBackend.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IMemoryCache _cache;
    private readonly IConfiguration _config;

    public UserService(
        AppDbContext context,
        IMemoryCache cache,
        IConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(cache);
        ArgumentNullException.ThrowIfNull(config);

        _context = context;
        _cache = cache;
        _config = config;
    }

    public async Task SendVerificationCodeAsync(string email)
    {
        // 1. 生成随机验证码（6位数字+字母组合）
        var code = ToolClass.GenerateRandomCode(6);

        // 2. 存储到数据库和缓存
        var verificationCode = new VerificationCode
        {
            Email = email,
            Code = code,
        };

        _context.VerificationCodes.Add(verificationCode);
        await _context.SaveChangesAsync();

        _cache.Set($"Code_{email}", code, TimeSpan.FromMinutes(5));

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("系统发件人", _config["Email:Account"]));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = "验证码通知";

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = $@"<p>您的验证码是：<strong>{code}</strong></p>
                                <p>有效期5分钟，请勿泄露给他人</p>";

        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(_config["Email:SmtpServer"],
                                int.Parse(_config["Email:Port"]!),
                                SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_config["Email:Account"],
                                      _config["Email:Password"]);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public async Task<string> UserRegisterAsync(string email, string code, string password)
    {
        var cout = await _context.Users.CountAsync(u => u.Username == email);
        if (cout > 0)
        {
            return await Task.FromResult("用户已存在");
        }

        var verificationCode = await _context.VerificationCodes
            .Where(v => v.Email == email &&
                        v.Code == code &&
                        v.ExpireTime > DateTime.Now &&
                        !v.IsUsed)
            .OrderBy(v => v.ExpireTime)
            .LastOrDefaultAsync();

        if (verificationCode is not null)
        {
            var user = new User();
            user.Username = email;
            user.PasswordSalt = ToolClass.GenerateSaltValue();
            user.PasswordHash = ToolClass.HashPassword(password, user.PasswordSalt);
            user.CreatedAt = DateTime.Now;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            verificationCode.IsUsed = true;
            await _context.SaveChangesAsync();
        }
        else
        {
            return await Task.FromResult("验证码错误");
        }
        return await Task.FromResult("注册成功");
    }
}
