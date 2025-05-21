using Ocelot.Configuration;
using Ocelot.DependencyInjection;
using Ocelot.ServiceDiscovery;
using Ocelot.ServiceDiscovery.Providers;
using Ocelot.Values;

namespace Dino.Gateway
{
    public static class OcelotAspireExtensions
    {
        public static IOcelotBuilder AddAspireServiceDiscovery(this IOcelotBuilder builder)
        {
            builder.Services
                .AddSingleton<ServiceDiscoveryFinderDelegate>((serviceProvider, config, downstreamRoute)
           => new AspireServiceDiscoveryProvider(serviceProvider, config, downstreamRoute));
            return builder;
        }
    }

    public class AspireServiceDiscoveryProvider : IServiceDiscoveryProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ServiceProviderConfiguration _serviceProviderConfiguration;
        private readonly DownstreamRoute _downstreamRoute;

        public AspireServiceDiscoveryProvider(IServiceProvider serviceProvider, ServiceProviderConfiguration configuration, DownstreamRoute downstreamRoute)
        {
            _serviceProvider = serviceProvider;
            _serviceProviderConfiguration = configuration;
            _downstreamRoute = downstreamRoute;
        }

        public async Task<List<Service>> GetAsync()
        {
            var services = new List<Service>();

            var serviceName = _downstreamRoute.ServiceName;

            var envVarKey = $"services__{serviceName}__http__0";
            var serviceUrl = Environment.GetEnvironmentVariable(envVarKey);
            if (!string.IsNullOrEmpty(serviceUrl))
            {
                var uri = new Uri(serviceUrl);
                services.Add(new Service(
                    serviceName,
                    new ServiceHostAndPort(uri.Host, uri.Port),
                    $"{serviceName}-1",
                    "1.0",
                    tags: ["downstream", "hardcoded"]
                ));
            }

            return await Task.FromResult(services);
        }
    }
}
