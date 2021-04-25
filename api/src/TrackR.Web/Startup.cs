using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TrackR.Api.Controllers;
using TrackR.Web.Configurations;

namespace TrackR.Web
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private const string ApiName = "TrackR Api";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .AddApplicationPart(typeof(AboutController).Assembly);
                    
            services.AddSwashbuckle(new OpenApiInfo
            {
                Title = ApiName,
                Version = SwashbuckleConfiguration.DefaultVersion
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseSwashbuckle(ApiName);

            app.UseRouting()
               .UseEndpoints(endpoints => endpoints.MapControllers())
               .UseSpa(spa =>
               {
                   spa.Options.SourcePath = "ClientApp";

                   if (env.IsEnvironment("Local"))
                   {
                       spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                   }
               });
        }
    }
}