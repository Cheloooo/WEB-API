using Microsoft.Extensions.Options;
using WEB_GATEWAY.Interfaces;
using WEB_GATEWAY.Models;

namespace WEB_GATEWAY.Services
{
    public class OutputCacheService : IOutputCacheServices
    {
        private readonly IReadOnlyDictionary<string, OutputCachePolicy> _policies;

        public OutputCacheService(IOptions<OutputCacheOptions> options)
        {
            _policies = options.Value.Policies;
        }
        public OutputCachePolicy? GetPolicy(string name)
        {
            _policies.TryGetValue(name, out var policy);
            return policy;
        }
    }
}
