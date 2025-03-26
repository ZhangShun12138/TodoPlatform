using TodoBackend.Models;

namespace TodoBackend.Interfaces;

public interface IUserService
{
    public Task SendVerificationCodeAsync(string email);

    public Task<string> UserRegisterAsync(string email, string code, string password);

    public Task<User?> AuthenticateUserAsync(string email, string password);
}
