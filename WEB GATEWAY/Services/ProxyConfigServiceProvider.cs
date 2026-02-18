using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using WEB_GATEWAY.Interfaces;
using WEB_GATEWAY.Models;
using Yarp.ReverseProxy.Configuration;
//configuration provider for yarp, it will read the configuration from appsettings and update the configuration when appsettings change
//how yarp knows when the configuration change? it uses a change token, when the configuration change,
//it will signal the change token, yarp will listen to the change token and update the configuration
namespace WEB_GATEWAY.Services
{ //sealed means prevents other classes from inheriting from this class, it can improve performance and security,
  //because it prevents other classes from modifying the behavior of this class it also implements IProxyConfigService,
  //which is an interface that defines the contract for getting the proxy configuration
    public sealed class ProxyConfigServiceProvider : IProxyConfigService
    { // this class uses an inner class InMemoryConfig to store the configuration, it has a volatile field _config to store the current configuration,
      // and it uses a cancellation token source to signal the change when the configuration change, it also has a method BuildConfig to build the configuration from the data read from appsettings
        private volatile InMemoryConfig _config;
        //volatile ensures that the value of _config is always read from the main memory, and not from a thread's local cache,
        //this is important because multiple threads may access the _config field concurrently, and we want to ensure that all threads see the most up-to-date value of _config
        public ProxyConfigServiceProvider(IOptionsMonitor<ReverseProxy> options)
        { //constructor is the heart of this class, it initializes the _config field with the initial configuration read from appsettings, and it also registers a callback to listen to the changes in the configuration,
         //when the configuration change, it will update the _config field and signal the change token
            _config = BuildConfig(options.CurrentValue);
            options.OnChange(updated =>
            { //Onchane is trigered when the configuration change, it will update the _config field and signal the change token
                //also notify yarp that the configuration has changed, so yarp can update its internal state accordingly
                _config = BuildConfig(updated);
                _config.SignalChange();
            });

        }
        public Task<IReadOnlyDictionary<string, ClusterConfig>> GetClustersAsync()
        {
            return Task.FromResult(_config.Clusters);
            //calls method internally to get the current configuration of clusters,
            //and returns it as a task, this allows the caller to await the result and get the configuration asynchronously
        }

        public Task<IReadOnlyDictionary<string, RouteConfig>> GetRoutesAsync()
        {
            return Task.FromResult(_config.Routes);
        }
        private static InMemoryConfig BuildConfig(ReverseProxy data)
        {
            var routes = data.Routes != null && data.Routes.Count > 0
                ? new Dictionary<string, RouteConfig>(data.Routes) 
                : new Dictionary<string, RouteConfig>();
            var clusters = data.Clusters != null && data.Clusters.Count > 0
                ? new Dictionary<string, ClusterConfig>(data.Clusters) 
                : new Dictionary<string, ClusterConfig>();
            return new InMemoryConfig(routes, clusters);
        }
        
        private sealed class InMemoryConfig(IReadOnlyDictionary<string, RouteConfig> routes, IReadOnlyDictionary<string, ClusterConfig> clusters)
        {
            public IReadOnlyDictionary<string, RouteConfig> Routes { get; } = routes;
            public IReadOnlyDictionary<string, ClusterConfig> Clusters { get; } = clusters;
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
