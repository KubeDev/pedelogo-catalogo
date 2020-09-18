using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PedeLogo.Catalogo.Api.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PedeLogo.Catalogo.Api.Middlewares
{
    public static class HealthMiddleware
    {
        public static void UseHealthMiddleware(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                if (ConfigManager.IsUnHealth())
                {
                    context.Response.StatusCode = 503;
                    await context.Response.WriteAsync("");
                }
                else
                {
                    await next.Invoke();
                }                
            });
        }
    }
}
