using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Learning
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //services.AddSingleton<ITelemetryInitializer>(new CloudRoleNameInitializer("imcodebased.com"));
            services.AddApplicationInsightsTelemetry(o => o.RequestCollectionOptions.EnableW3CDistributedTracing = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            var builder = TelemetryConfiguration.Active.TelemetryProcessorChainBuilder;
            builder.Use((next) => new SkipQuickDependencies(500, "SQL", next));
            builder.Build();
        }

        public class CloudRoleNameInitializer : ITelemetryInitializer
        {
            private readonly string roleName;

            public CloudRoleNameInitializer(string roleName)
            {
                this.roleName = roleName ?? throw new ArgumentNullException(nameof(roleName));
            }

            public void Initialize(ITelemetry telemetry)
            {
                telemetry.Context.Cloud.RoleName = this.roleName;
                telemetry.Context.Cloud.RoleInstance = this.roleName;
            }
        }
    }
}
