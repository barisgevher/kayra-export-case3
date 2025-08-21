namespace Shared.Infrastructure.Security
{
    public class RedisTokenRevocationService : ITokenRevocationService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<RedisTokenRevocationService> _logger;
        private const string REVOKED_TOKENS_PREFIX = "revoked_token:";
        private const string USER_TOKENS_PREFIX = "user_tokens:";

        public RedisTokenRevocationService(IDistributedCache cache, ILogger<RedisTokenRevocationService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task RevokeTokenAsync(string jti, DateTime expiry, CancellationToken cancellationToken = default)
        {
            var key = $"{REVOKED_TOKENS_PREFIX}{jti}";
            var revokedToken = new RevokedToken
            {
                Jti = jti,
                RevokedAt = DateTime.UtcNow,
                ExpiresAt = expiry
            };

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = expiry.AddMinutes(5) 
            };

            await _cache.SetStringAsync(key, JsonSerializer.Serialize(revokedToken), options, cancellationToken);
            _logger.LogInformation("Token revoked: {Jti}", jti);
        }

        public async Task RevokeAllUserTokensAsync(string userId, CancellationToken cancellationToken = default)
        {
            var key = $"{USER_TOKENS_PREFIX}{userId}";
            var revokeAllEntry = new UserTokenRevocation
            {
                UserId = userId,
                RevokedAt = DateTime.UtcNow
            };

            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromDays(30)
            };

            await _cache.SetStringAsync(key, JsonSerializer.Serialize(revokeAllEntry), options, cancellationToken);
            _logger.LogInformation("All tokens revoked for user: {UserId}", userId);
        }

        public async Task<bool> IsTokenRevokedAsync(string jti, CancellationToken cancellationToken = default)
        {
           
            var tokenKey = $"{REVOKED_TOKENS_PREFIX}{jti}";
            var revokedToken = await _cache.GetStringAsync(tokenKey, cancellationToken);

            if (!string.IsNullOrEmpty(revokedToken))
            {
                return true;
            }

          

            return false;
        }

        public async Task CleanupExpiredTokensAsync(CancellationToken cancellationToken = default)
        {
           
            _logger.LogInformation("Token cleanup completed");
            await Task.CompletedTask;
        }
    }

    public class RevokedToken
    {
        public string Jti { get; set; } = string.Empty;
        public DateTime RevokedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    public class UserTokenRevocation
    {
        public string UserId { get; set; } = string.Empty;
        public DateTime RevokedAt { get; set; }
    }
}