using School_View_Models;

namespace School_IServices
{
    public interface IAccountService
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<bool> IsUserExsit(string Username, string Password);
        string GenerateJwtToken(string name, string email);
        Task<bool> Register(UserViewModel registerModel);
    }
}
