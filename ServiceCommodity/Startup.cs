using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Consul;
using Newtonsoft.Json;

namespace ServiceCommodity
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
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
           ServiceRegister(Configuration);
        }
        private static void ServiceRegister(IConfiguration configuration)
        {

            ConsulClient client = new ConsulClient(new Action<ConsulClientConfiguration>(t=> {
                t.Address = new Uri(configuration["consul:servicesAddr"]);
                t.Datacenter = configuration["consul:datacenter"];
            }));
            Console.WriteLine(configuration["port"]);
            var result = client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                Address = configuration["ip"],
                ID = $"{configuration["consul:serviceName"]}{configuration["id"]}",
                Name = configuration["consul:serviceName"],
                Port = Convert.ToInt32(configuration["port"]),
                Tags = null,
                Check = new AgentServiceCheck()
                {
                    HTTP =$"http://{configuration["ip"]}:{configuration["port"]}{configuration["consul:healthCheck"]}",
                    Interval = new TimeSpan(0, 0, 10),
                    DeregisterCriticalServiceAfter = new TimeSpan(0, 1, 0),
                }
            }).Result;
        }

    }
}
