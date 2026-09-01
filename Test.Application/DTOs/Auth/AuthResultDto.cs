namespace Test.Application.DTOs.Auth
{
    public record AuthResultDto(string Token, DateTime ExpiresAtUtc, string Email, IEnumerable<string> Roles);
}