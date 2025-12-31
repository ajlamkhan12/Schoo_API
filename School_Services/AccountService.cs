using AutoMapper;
using Data.Data;
using DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using School_IServices;
using School_View_Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace School_Services
{
    public class AccountService : IAccountService
    {
        private readonly School_ManagementContext _context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public AccountService(School_ManagementContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _config = configuration;
        }
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == request.Username.ToLower() && x.Password.ToLower() == request.Password.ToLower());
            if (user == null)
            {
                return null;
            }
            var token = GenerateJwtToken(user.Name,user.Email);
            return new LoginResponse
            {
                Token = token,
                User = new LoginViewModel
                {
                    UserId = user.Id,
                    Name = request.Username,
                    Role = "admin",
                    Email = "admin@gmail.com"
                }
            };
        }

        public async Task<bool> IsUserExsit(string Username, string Password)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == Username.ToLower() && x.Password.ToLower() == Password.ToLower());
                if (user == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<UserViewModel> Register(UserViewModel registerModel)
        {
            var user = _mapper.Map<User>(registerModel);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            registerModel = _mapper.Map<UserViewModel>(user);
            return registerModel;
        }

        public string GenerateJwtToken(string name, string email)
        {
            try
            {
                var claims = new[]
           {
            new Claim(JwtRegisteredClaimNames.Sub, "admin"),
            new Claim("name",name),
            new Claim("role", "admin"),
            new Claim("email", email)
        };
                var secretKey = "School_app_super_secret_key_12345";
                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(secretKey)
                );

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                     issuer: _config["Jwt:Issuer"],
                     audience: _config["Jwt:Audience"],
                     claims: claims,
                     expires: DateTime.UtcNow.AddHours(
                         Convert.ToDouble(_config["Jwt:ExpiryMinutes"])
                     ),
                     signingCredentials: creds
                 );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
