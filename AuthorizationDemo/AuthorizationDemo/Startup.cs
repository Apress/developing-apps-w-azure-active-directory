using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CommonLibrary;
using AuthorizationDemo.Authorization.Requirements;
using AuthorizationDemo.Authorization.AuthorizationHandlers;
using AuthorizationDemo.Services;

namespace AuthorizationDemo
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
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddAzureAd(options =>
                {
                    Configuration.Bind("AzureADSettings", options);
                    AzureADSettings.AzureSettings = options;
                })
            .AddCookie();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminGroupPolicy", policy => policy.RequireClaim("groups", "0a611d07-7b30-4bf2-8d60-263d3f008188"));
                options.AddPolicy("HRAdminPolicy", policy => policy.Requirements.Add(new EmployeeDepartmentRequirement("HR")));
                options.AddPolicy("HROperationsReadPolicy", policy => policy.Requirements.Add(new ResourcePermissionsRequirement("HROperations", "read")));
                options.AddPolicy("HROperationsWritePolicy", policy => policy.Requirements.Add(new ResourcePermissionsRequirement("HROperations", "write")));
            });

            services.AddSingleton<IAuthorizationHandler, EmployeeDepartmentHandler>();
            services.AddSingleton<IAuthorizationHandler, ResourcePermissionsHandler>();
            services.AddSingleton<IMockEmployeeService, MockEmployeeService>();
            services.AddMvc()
                .AddSessionStateTempDataProvider();
            services.AddSession();
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
                app.UseExceptionHandler("/Home/Error");
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
