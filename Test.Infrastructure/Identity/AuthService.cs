using Microsoft.AspNetCore.Identity;

using Test.Application.DTOs.Auth;
using Test.Application.Exceptions;
using Test.Application.Interfaces.Services;
using Test.Domain.Constants;

namespace Test.Infrastructure.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResultDto> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken = default)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new ArgumentException(string.Join("; ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, AppRoles.User);

            var (token, expiresAt) = _tokenService.GenerateToken(user, new[] { AppRoles.User });
            return new AuthResultDto(token, expiresAt, user.Email, new[] { AppRoles.User });
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new AuthenticationFailedException("Invalid email or password.");

            var roles = await _userManager.GetRolesAsync(user);
            var (token, expiresAt) = _tokenService.GenerateToken(user, roles);
            return new AuthResultDto(token, expiresAt, user.Email!, roles);
        }
    }
}