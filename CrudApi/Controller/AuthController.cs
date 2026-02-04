using Microsoft.EntityFrameworkCore;
using CrudApi.Data;
using CrudApi.DTO;
using CrudApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CrudApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }


        [HttpPost("Register")]

        public async Task<IActionResult> Register(Register model)
        {
            // 1️⃣ Check if username exists
            if (await _context.Registers.AnyAsync(u => u.Username == model.Username))
                return BadRequest("Username already exists");

            // 2️⃣ Create password hash & salt
            CreatePasswordHash(model.PasswordHashAsString(), out byte[] passwordHash, out byte[] passwordSalt);

            // 3️⃣ Assign hash and salt to model
            model.PasswordHash = passwordHash;
            model.PasswordSalt = passwordSalt;

            // Ensure role is set
            if (string.IsNullOrEmpty(model.Role))
                model.Role = "User";

            // 4️⃣ Save to database
            _context.Registers.Add(model);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }


        [HttpPost("login")]
        public IActionResult Login(LoginDto login)
        {
            if (login.Username != "admin" || login.Password != "password")
                return Unauthorized("Invalid credentials");

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, login.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        // Helper method to hash password
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

    }
}
