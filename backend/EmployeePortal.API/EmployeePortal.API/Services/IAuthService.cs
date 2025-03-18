using EmployeePortal.API.DTOs;

namespace EmployeePortal.API.Services
{
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(LoginRequest request);
        Task<AuthResult> RegisterAsync(RegisterRequest request);
    }
}
