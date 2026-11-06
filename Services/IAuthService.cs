using TaskManagement.Models;
using TaskManagement.DTO;

namespace TaskManagement.Services
{
    public interface IAuthService
    {
        bool Register(RegisterDTO registerDTO);

        string? Login(LoginDTO loginDTO);
    }
}