using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using tt_apps_srs.Models;
using tt_apps_srs.Lib;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;

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
            services.AddMvc();

            // Adding ES Client as db change auditor
            services.AddSingleton<IAuditor>( s => new Auditor(Configuration.GetConnectionString("DefaultESConnection")));
            services.AddEntityFrameworkMySql();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IClientProvider, ClientProvider>();
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
                app.UseFileServer(new FileServerOptions {
                    FileProvider = new PhysicalFileProvider(
                                        Path.Combine(Directory.GetCurrentDirectory(), @"node_modules")),
                    RequestPath = new PathString("/node_modules"),
                    EnableDirectoryBrowsing = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();
            app.UseSession();
            app.UseMvcWithDefaultRoute();
        }
    }
}
