using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Prometheus;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using PedeLogo.Catalogo.Api.Config;
using Microsoft.AspNetCore.Http;
using PedeLogo.Catalogo.Api.Middlewares;

namespace PedeLogo.Catalogo.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = $"mongodb://{this.Configuration.GetSection("Mongo:User").Get<string>()}:{this.Configuration.GetSection("Mongo:Password").Get<string>()}@{this.Configuration.GetSection("Mongo:Host").Get<string>()}:{this.Configuration.GetSection("Mongo:Port").Get<string>()}/{this.Configuration.GetSection("Mongo:DataBase").Get<string>()}";
            services.AddScoped<IMongoDatabase>(sp => new MongoClient(connectionString)
                .GetDatabase((this.Configuration.GetSection("Mongo:DataBase").Get<string>())));

            services.AddControllers();

            services.AddHealthChecks()
                 .AddCheck("Health Test", () =>
                 {
                     if (ConfigManager.IsUnHealth())
                     {
                         return HealthCheckResult.Unhealthy();
                     }
                     else
                     {
                         return HealthCheckResult.Healthy();
                     }
                 }
                 , tags: new[] { "Health" });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API para cadastro de Produtos",
                    Description = "API para cadastro de Produtos"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catálogo de produtos API");
            });

            app.UseHealthMiddleware();

            app.UseRouting();

            app.UseHttpMetrics();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics();
                endpoints.MapGet("/read", async context =>
                {
                    if (ConfigManager.IsRead())
                    {
                        context.Response.StatusCode = 200;
                        await context.Response.WriteAsync("");
                    }
                    else
                    {
                        context.Response.StatusCode = 503;
                        await context.Response.WriteAsync("");
                    }
                });
            });
        }
    }
}