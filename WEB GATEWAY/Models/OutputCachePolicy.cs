using System;

namespace WEB_GATEWAY.Models
{
    public class OutputCachePolicy
    {
        public TimeSpan Duration { get; set; }
    }
    public class OutputCacheOptions
    {
        public IReadOnlyDictionary<string, OutputCachePolicy> Policies { get; set; } 
    }
}
