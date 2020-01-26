using Amazon.SimpleNotificationService;
using AWS_SQS_NET_Core_Example.Core.Commands;
using AWS_SQS_NET_Core_Example.Database;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Amazon.SQS;
using AWS_SQS_NET_Core_Example.HostedServices;

namespace AWS_SQS_NET_Core_Example
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

            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonSimpleNotificationService>();
            services.AddAWSService<IAmazonSQS>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddMediatR(typeof(CreateCustomerCommand));

            services.AddSingleton<AWSSQSHelper>();

            services.AddHostedService<Worker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
