using Yarp.ReverseProxy.Configuration;

namespace WEB_GATEWAY.Models
{
    public class ReverseProxy
    {
        public IReadOnlyDictionary<string, RouteConfig> Routes { get; set; }
        public IReadOnlyDictionary<string, ClusterConfig> Clusters { get; set; }
    }
}
