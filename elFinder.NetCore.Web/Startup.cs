﻿using System.IO;
using elFinder.NetCore.Drivers.AzureStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace elFinder.NetCore.Web
{
    public class Startup
    {
        public static string WebRootPath { get; private set; }

        public static string MapPath(string path, string basePath = null)
        {
            if (string.IsNullOrEmpty(basePath))
            {
                basePath = WebRootPath;
            }

            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            return Path.Combine(basePath, path);
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                app.UseHsts();
            }

            // Setup Azure Storage Credentials
            AzureStorageAPI.AccountName = "[Name]";
            AzureStorageAPI.AccountKey = "[Key]";

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            WebRootPath = env.WebRootPath;
        }
    }
}