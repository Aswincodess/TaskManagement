using Microsoft.AspNetCore.Mvc;
using TaskManagement.DTO;
using TaskManagement.Services;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

     
        [HttpPost("register")]
        public IActionResult Register(RegisterDTO registerDTO)
        {
            var result = _authService.Register(registerDTO);

            if (!result)
            {
                return Conflict("Email already exists");
            }

            return Ok("Registration successful");
        }

       
        [HttpPost("login")]
        public IActionResult Login(LoginDTO loginDTO)
        {
            var token = _authService.Login(loginDTO);

            if (token == null)
            {
                return Unauthorized("Invalid email or password");
            }

            return Ok(new { Token = token });
        }
    }
}