using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using tt_apps_srs.Models;

namespace tt_apps_srs
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
            // Adding Session
            services.AddSession();

            // Adding MVC
            services.AddRouting();
            services
                .AddMvc();

            // Adding ES Client as db change auditor
            services.AddSingleton<IAuditor>( s => new Auditor(Configuration.GetConnectionString("DefaultESConnection")));
            services.AddEntityFrameworkMySql();
            services.AddDbContextPool<tt_apps_srs_db_context>((serviceProvider, options) =>
            {
                options.UseMySql(Configuration.GetConnectionString("DefaultDbConnection"));
                // DI ES auditor to DB Context
                options.UseInternalServiceProvider(serviceProvider);
            });
            

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
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "TenantManagement",
                    template: "Tenant/{page}/{id?}"
                );
                routes.MapRoute(
                    name: "TenantBased",
                    template: "{tenant_url_code}/{controller}/{action}/{id?}"
                );
            });
        }
    }
}
