namespace TodoBackend.tools;

public class JwtTokenSettings
{
    public string SiteUrl { get; set; } = default!;
    public string SecretKey { get; set; } = default!;
    public int TokenExpires { get; set; } // 单位：小时
    public int UserExpiredAt { get; set; } // 单位：分钟
}
