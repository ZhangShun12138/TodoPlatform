using System.Security.Cryptography;
using System.Text;

namespace ClassLibrary;

public static class ToolClass
{
    /// <summary>
    /// 创建密码哈希值和盐值
    /// </summary>
    /// <param name="password">密码</param>
    /// <param name="hash">哈希值</param>
    /// <param name="salt">盐</param>
    public static void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
    {
        using var hmac = new HMACSHA512();
        salt = hmac.Key;
        hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    /// <summary>
    /// 生成随机数字验证码
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string GenerateRandomCode(int length)
    {
        const string chars = "0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
    }

    /// <summary>
    /// 随机生成盐值
    /// </summary>
    /// <param name="saltSize"></param>
    /// <returns></returns>
    public static byte[] GenerateSaltValue(int saltSize = 16) // 默认16字节盐值
    {
        using var rng = RandomNumberGenerator.Create(); // 使用加密安全的随机数生成器
        byte[] salt = new byte[saltSize];
        rng.GetBytes(salt);
        return salt;
    }

    /// <summary>
    /// 计算密码的哈希值
    /// </summary>
    /// <param name="password"></param>
    /// <param name="salt"></param>
    /// <returns></returns>
    public static byte[] HashPassword(string password, byte[] salt)
    {
        using var hashAlgorithm = SHA256.Create(); // 可根据需要替换为PBKDF2等更安全的算法
                                                   // 将密码明文与盐值合并
        var passwordBytes = Encoding.Unicode.GetBytes(password); // UTF-16编码[1](@ref)
        var combinedBytes = new byte[passwordBytes.Length + salt.Length];
        Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
        Buffer.BlockCopy(salt, 0, combinedBytes, passwordBytes.Length, salt.Length);
        // 计算哈希值
        return hashAlgorithm.ComputeHash(combinedBytes);
    }
}
