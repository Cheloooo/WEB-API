using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using WEB_GATEWAY.Interfaces;
using WEB_GATEWAY.Models;

namespace WEB_GATEWAY.Services
{
    public sealed class RateLimitConfigServiceProvider : IRateLimitConfigServiceProvider
    {
        private volatile InMemoryConfig _config;
        private readonly IMemoryCache _cache;

        public RateLimitConfigServiceProvider(IOptionsMonitor<RateLimitingConfig> options)
        {
            _config = BuildConfig(options.CurrentValue);
            options.OnChange(updated =>
            {
                _config = BuildConfig(updated);
                _config.SignalChange();
            });
        }
        public IReadOnlyDictionary<string, RateLimitOptions> GetAllPolicies()
        {
           return _config.Policies;
        }

        public RateLimitOptions? GetPolicy(string policyName)
        {
            return _config.Policies.TryGetValue(policyName, out var policy) ? policy : null;
        }

        public bool IsRequestAllowed(string policyName, string clientId)
        {
            if (!_config.Policies.TryGetValue(policyName, out var policy))
               return true; // No policy means no limit
            var key = $"{policyName}:{clientId}";
            var now = DateTime.UtcNow;
            var entry = _cache.GetOrCreate(key, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(policy.WindowSeconds);
                return new List<DateTime>();
            });
            var timestamps = (List<DateTime>)entry!;
            timestamps.RemoveAll(t => (now - t).TotalSeconds > policy.WindowSeconds);

            if(timestamps.Count >= policy.PermitLimit)
            {
                return false; // Rate limit exceeded
            }
            timestamps.Add(now);
            _cache.Set(key, timestamps, TimeSpan.FromSeconds(policy.WindowSeconds));
            return true; // Request allowed
        }
        private static InMemoryConfig BuildConfig(RateLimitingConfig data)
        {
            return new(data);
        }

        private sealed class InMemoryConfig(RateLimitingConfig config) : IRateLimitConfig
        {
            public IReadOnlyDictionary<string, RateLimitOptions> Policies { get; } = config.Policies != null
                ? new Dictionary<string, RateLimitOptions>(config.Policies)
                : [];
            private CancellationTokenSource _cts = new();
            public IChangeToken ChangeToken => new CancellationChangeToken(_cts.Token);
            public void SignalChange()
            {
                var previousCts = Interlocked.Exchange(ref _cts, new CancellationTokenSource());
                previousCts.Cancel();
            }
        }
    }
}
