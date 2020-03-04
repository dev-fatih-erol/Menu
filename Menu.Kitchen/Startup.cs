using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Menu.Data;
using Menu.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Menu.Kitchen
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.Cookie.Name = "kitchen";
                        options.LoginPath = "/";
                        options.LogoutPath = "/";
                        options.AccessDeniedPath = "/";
                    });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("tr-TR", "tr-TR");
            });

            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<IKitchenService, KitchenService>();

            services.AddScoped<ITableWaiterService, TableWaiterService>();

            services.AddScoped<IUserTokenService, UserTokenService>();

            services.AddScoped<ITableService, TableService>();

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddAntiforgery(options => options.Cookie.Name = "x-csrf-token");

            services.AddDbContext<MenuContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDataProtection();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRequestLocalization();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}