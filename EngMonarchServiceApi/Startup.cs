using EngMonarchApi.Data;
using EngMonarchApi.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSwag.AspNetCore;
using Microsoft.AspNetCore.Http;

namespace EngMonarchApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IEngMonarchRepository, EngMonarchRepository>();

            services.AddJwtBearerAuthentication(Configuration);

            services.AddCors();

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = "127.0.0.1:6379";
            });
            services.AddScoped<ETagRedisCache>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc();
           
            services.AddVersioning();

            services.AddSwagger();

            services.AddHealthChecks();

            services.AddResponseCompression();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env
            , ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            loggerFactory.AddFile("Logs/EngMonarchServiceApi-{Date}.txt");

            // Inject our custom error handling middleware into ASP.NET Core pipeline
            app.UseMiddleware<ErrorHandlingMiddleware>();
           

            app.UseCors(builder =>
                builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
                       
            app.UseAuthentication();
            app.UseResponseCompression();

            app.UseMiddleware<LimitingMiddleware>();

            //app.UseHttpsRedirection();
            
            // Use Cookie Policy Middleware to conform to EU General Data 
            // Protection Regulation (GDPR) regulations.
            app.UseCookiePolicy();

            app.UseSwagger();

            app.UseHealthChecks("/CheckHealth");

            

            app.UseMvc();
        }
    }
}
