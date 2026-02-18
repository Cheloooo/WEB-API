using Yarp.ReverseProxy.Configuration;

namespace WEB_GATEWAY.Interfaces
{
    public interface IProxyConfigService
    {
        Task<IReadOnlyDictionary<string, RouteConfig>> GetRoutesAsync();
        Task<IReadOnlyDictionary<string, ClusterConfig>> GetClustersAsync();
    }
}
