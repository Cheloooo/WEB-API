using WEB_GATEWAY.Models;

namespace WEB_GATEWAY.Interfaces
{
    public interface IRateLimitConfig
    {
        IReadOnlyDictionary<string, RateLimitOptions> Policies { get; }
    }
}
