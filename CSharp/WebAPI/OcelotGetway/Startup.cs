using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

namespace OcelotGetway
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication("TestKey", options =>
                {
                    options.Authority = "http://localhost:6000";
                    options.RequireHttpsMetadata = false;
                    options.ApiName = "api1";
                });
            services.AddMvc();
            services.AddOcelot(new ConfigurationBuilder()
                .AddJsonFile("configuration.json")
                .Build()).AddConsul();
        }

        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            await app.UseOcelot();
            app.UseMvc();
        }

    }
}