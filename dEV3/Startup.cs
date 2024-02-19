using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace dEV3
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddTransient<ITimeService, ShortTimeService>();
            services.AddTransient<ICalcService, CalcService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Add middleware for handling time-related requests
            app.UseMiddleware<TimeMiddleware>();

            // Add middleware for handling calculator-related requests
            app.UseMiddleware<CalcServiceMiddleware>();

            app.UseEndpoints(endpoints =>
            {

                endpoints.Map("/Time", async context =>
                {
                    await context.Response.WriteAsync("This is the Time endpoint.");
                });

                endpoints.Map("/Calc", async context =>
                {
                    await context.Response.WriteAsync("This is the Calc endpoint.");
                });
            });
        }
    }
}



