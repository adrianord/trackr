using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrackR.Web.Helpers;

namespace TrackR.Web.Configurations
{
    public static class CommonConfigurations
    {
        public static void AddCommonServices(this IServiceCollection services, IConfiguration configuration) =>
            services.AddMediatR(AssemblyScanning.GetAssemblies())
                    .AddAutoMapper(AssemblyScanning.GetAssemblies());
    }
}