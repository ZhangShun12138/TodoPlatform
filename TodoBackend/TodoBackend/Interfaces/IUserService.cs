namespace TodoBackend.Interfaces;

public interface IUserService
{
    public Task SendVerificationCodeAsync(string email);

    public Task<string> UserRegisterAsync(string email, string code, string password);
}
