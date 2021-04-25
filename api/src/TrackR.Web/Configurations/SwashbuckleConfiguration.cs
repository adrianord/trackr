using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace TrackR.Web.Configurations
{
    public static class SwashbuckleConfiguration
    {
        public const string DefaultVersion = "v1";

        public static IServiceCollection AddSwashbuckle(this IServiceCollection services, OpenApiInfo apiInfo) =>
            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(x => x.FullName); // Prevents conflicts with commonly named parameters
                c.DescribeAllParametersInCamelCase(); // Follow http parameter naming convention
                c.SwaggerDoc(apiInfo.Version, apiInfo);
                c.EnableAnnotations();
            });

        public static void UseSwashbuckle(this IApplicationBuilder app, string name,
            string apiVersion = DefaultVersion) =>
            app.UseSwagger()
               .UseSwaggerUI(c =>
                   c.SwaggerEndpoint($"/swagger/{apiVersion}/swagger.json", $"{name} {apiVersion.ToUpper()}"));
    }
}