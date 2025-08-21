namespace Shared.Infrastructure.Security
{
    public interface ITokenRevocationService
    {
        Task RevokeTokenAsync(string jti, DateTime expiry, CancellationToken cancellationToken = default);
        Task RevokeAllUserTokensAsync(string userId, CancellationToken cancellationToken = default);
        Task<bool> IsTokenRevokedAsync(string jti, CancellationToken cancellationToken = default);
        Task CleanupExpiredTokensAsync(CancellationToken cancellationToken = default);
    }
}
