using AutoMapper;
using Azure.Core;
using Data.Data;
using DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using School_IServices;
using School_View_Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace School_Management_System.Controllers
{
    [Authorize]
    [Route("api/auth")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly School_ManagementContext _context;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        public AccountController(School_ManagementContext context, IMapper mapper, IAccountService accountService)
        {
            _context = context;
            _mapper = mapper;
            _accountService = accountService;
        }
        #region Public
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                bool isExsit = await _accountService.IsUserExsit(request.Username, request.Password);
                if (isExsit)
                {
                    return Ok(await _accountService.Login(request));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserViewModel registerModel)
        {
            try
            {
                bool isExsit = await _accountService.IsUserExsit(registerModel.UserName, registerModel.Password);
                if (isExsit)
                {
                    return Conflict();
                }
                if(await _accountService.Register(registerModel))
                {
                    string token = _accountService.GenerateJwtToken(registerModel.Name, registerModel.Email);
                    return Ok (new LoginResponse
                    {
                        Token = token,

                        User = new LoginViewModel
                        {
                            Name = registerModel.UserName,
                            Role = "admin",
                            Email = registerModel.Email
                        }
                    });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var user = await _context.Users.ToListAsync();
                return Ok(user);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Private

        private string GenerateJwtToken()
        {
            try
            {
                var claims = new[]
           {
            new Claim(JwtRegisteredClaimNames.Sub, "admin"),
            new Claim("name","admin"),
            new Claim("role", "admin"),
            new Claim("email", "admin@gmail.com")
        };
                var secretKey = "School_app_super_secret_key_123";
                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(secretKey)
                );

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "School_app",
                    audience: "School_app",
                    claims: claims,
                    expires: DateTime.Now.AddHours(2),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        #endregion

    }


}
