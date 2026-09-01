using Test.Application.DTOs.Auth;

namespace Test.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResultDto> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken = default);
        Task<AuthResultDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default);
    }
}