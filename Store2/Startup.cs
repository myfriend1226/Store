using System;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Store2.Data;
using Store2.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Store2.Services;

namespace Store2
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

        
            // Get Identity Default Options
            IConfigurationSection identityDefaultOptionsConfigurationSection = Configuration.GetSection("IdentityDefaultOptions");

            services.Configure<IdentityDefaultOptions>(identityDefaultOptionsConfigurationSection);

            var identityDefaultOptions = identityDefaultOptionsConfigurationSection.Get<IdentityDefaultOptions>();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    // Password settings
                    options.Password.RequireDigit = identityDefaultOptions.PasswordRequireDigit;
                    options.Password.RequiredLength = identityDefaultOptions.PasswordRequiredLength;
                    options.Password.RequireNonAlphanumeric = identityDefaultOptions.PasswordRequireNonAlphanumeric;
                    options.Password.RequireUppercase = identityDefaultOptions.PasswordRequireUppercase;
                    options.Password.RequireLowercase = identityDefaultOptions.PasswordRequireLowercase;
                    options.Password.RequiredUniqueChars = identityDefaultOptions.PasswordRequiredUniqueChars;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(identityDefaultOptions.LockoutDefaultLockoutTimeSpanInMinutes);
                    options.Lockout.MaxFailedAccessAttempts = identityDefaultOptions.LockoutMaxFailedAccessAttempts;
                    options.Lockout.AllowedForNewUsers = identityDefaultOptions.LockoutAllowedForNewUsers;

                    // User settings
                    options.User.RequireUniqueEmail = identityDefaultOptions.UserRequireUniqueEmail;

                    // email confirmation require
                    options.SignIn.RequireConfirmedEmail = identityDefaultOptions.SignInRequireConfirmedEmail;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
            services.AddAuthentication()
                .AddIdentityServerJwt();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.Configure<SendGridOptions>(Configuration.GetSection("SendGridOptions"));

            // Get SMTP configuration options
            services.Configure<SmtpOptions>(Configuration.GetSection("SmtpOptions"));

            // Get Super Admin Default options
            services.Configure<SuperAdminDefaultOptions>(Configuration.GetSection("SuperAdminDefaultOptions"));

            // Add email services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddTransient<INumberSequence, Services.NumberSequence>();

            services.AddTransient<IRoles, Roles>();

            services.AddTransient<IFunctional, Functional>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAutoMapper(typeof(Startup));
            /*services.AddMvc().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.PropertyNamingPolicy = null;
                o.JsonSerializerOptions.DictionaryKeyPolicy = null;
            });*/
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
