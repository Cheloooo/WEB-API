using WEB_GATEWAY.Interfaces;

namespace WEB_GATEWAY.Models
{
    public class RateLimitingConfig : IRateLimitConfig
    {
        public IReadOnlyDictionary<string, RateLimitOptions> Policies { get; set; } = new Dictionary<string, RateLimitOptions>();
    }
}
