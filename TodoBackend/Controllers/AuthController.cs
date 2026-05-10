using Microsoft.AspNetCore.Mvc;
using TodoBackend.Data;
using TodoBackend.DTOs;
using TodoBackend.Models;
using TodoBackend.Services;

namespace TodoBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(
            AppDbContext context,
            JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDTO dto)
        {
            var userExists = _context.Users
                .Any(x => x.Email == dto.Email);

            if (userExists)
            {
                return BadRequest(
                    new { message = "Email already exists" });
            }

            var user = new User
            {
                Email = dto.Email,
                Password = dto.Password
            };

            _context.Users.Add(user);

            _context.SaveChanges();

            return Ok(new
            {
                message = "Register success"
            });
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO dto)
        {
            var user = _context.Users.FirstOrDefault(x =>
                x.Email == dto.Email &&
                x.Password == dto.Password);

            if (user == null)
            {
                return Unauthorized(
                    new { message = "Invalid email or password" });
            }

            var token =
                _jwtService.GenerateToken(user.Id);

            return Ok(new
            {
                token = token
            });
        }
    }
}