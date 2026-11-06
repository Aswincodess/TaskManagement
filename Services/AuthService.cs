using TaskManagement.Models;
using TaskManagement.DTO;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TaskManagement.Services
{
    public class AuthService : IAuthService
    {
        private readonly List<User> _users = new List<User>();

        private int _nextId = 1;

        private readonly PasswordHasher<User> _passwordHasher =
            new PasswordHasher<User>();

        public bool Register(RegisterDTO registerDTO)
        {
         
            var existingUser = _users.FirstOrDefault(
                u => u.Email == registerDTO.Email);

            if (existingUser != null)
            {
                return false;
            }

           
            var user = new User
            {
                Id = _nextId++,
                Username = registerDTO.Username,
                Email = registerDTO.Email,
                Role = "User"
            };

            user.PasswordHash = _passwordHasher.HashPassword(
                user, registerDTO.Password);

            _users.Add(user);

            return true;
        }

        public string? Login(LoginDTO loginDTO)
        {
           
            var user = _users.FirstOrDefault(
                u => u.Email == loginDTO.Email);

            if (user == null)
            {
                return null;
            }

           
            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                loginDTO.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

           
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("ThisIsADevelopmentSecretKeyAtLeast32Characters!"));

            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "TaskManagement",
                audience: "TaskManagementUsers",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}