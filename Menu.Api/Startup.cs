using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Menu.Api.Services;
using Menu.Data;
using Menu.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Menu.Api
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
            var appSettings = Configuration.GetSection("AppSettings");

            var secret = appSettings["Secret"];

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("tr-TR", "tr-TR");
            });

            services.AddAuthentication(a =>
            {
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(a =>
            {
                a.RequireHttpsMetadata = false;
                a.SaveToken = true;
                a.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddScoped<IAppAboutService, AppAboutService>();

            services.AddScoped<IAppSliderService, AppSliderService>();

            services.AddScoped<ICityService, CityService>();

            services.AddScoped<IVenueService, VenueService>();

            services.AddScoped<ITableService, TableService>();

            services.AddScoped<IWaiterService, WaiterService>();

            services.AddScoped<ITableWaiterService, TableWaiterService>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IOptionService, OptionService>();

            services.AddScoped<IOptionItemService, OptionItemService>();

            services.AddScoped<IOrderTableService, OrderTableService>();

            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<IOrderDetailService, OrderDetailService>();

            services.AddScoped<IOrderPaymentService, OrderPaymentService>();

            services.AddScoped<IOrderCashService, OrderCashService>();

            services.AddScoped<IOrderWaiterService, OrderWaiterService>();

            services.AddScoped<IVenuePaymentMethodService, VenuePaymentMethodService>();

            services.AddScoped<ICommentRatingService, CommentRatingService>();

            services.AddScoped<IFavoriteService, FavoriteService>();

            services.AddScoped<IVenueFeatureService, VenueFeatureService>();

            services.AddScoped<ISuggestionComplaintService, SuggestionComplaintService>();

            services.AddScoped<IWaiterTokenService, WaiterTokenService>();

            services.AddScoped<IUserTokenService, UserTokenService>();

            services.AddScoped<ICashService, CashService>();

            services.AddScoped<INotificationWaiterSubjectService, NotificationWaiterSubjectService>();

            services.AddScoped<INotificationWaiterService, NotificationWaiterService>();

            services.AddTransient<ISmsSender, SmsSender>();

            services.AddSingleton(Configuration);

            services.AddDbContext<MenuContext>(o => o.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddAutoMapper(typeof(Startup));

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                        builder => builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader());
            });

            services.AddSignalR();

            services.AddDataProtection();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseRequestLocalization();

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}