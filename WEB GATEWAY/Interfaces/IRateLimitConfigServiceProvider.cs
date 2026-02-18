using WEB_GATEWAY.Models;

namespace WEB_GATEWAY.Interfaces
{
    public interface IRateLimitConfigServiceProvider
    {
        RateLimitOptions? GetPolicy(string policyName);
        IReadOnlyDictionary<string, RateLimitOptions> GetAllPolicies();
        bool IsRequestAllowed(string policyName, string clientId);
    }
}
