
namespace School_View_Models
{
    public class LoginViewModel
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }

    // DTOs/LoginRequest.cs
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    // DTOs/LoginResponse.cs
    public class LoginResponse
    {
        public string Token { get; set; }
        public LoginViewModel User { get; set; }
    }
}
