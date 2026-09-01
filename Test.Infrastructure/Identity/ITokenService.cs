namespace Test.Infrastructure.Identity
{
    public interface ITokenService
    {
        (string Token, DateTime ExpiresAtUtc) GenerateToken(ApplicationUser user, IList<string> roles);
    }
}