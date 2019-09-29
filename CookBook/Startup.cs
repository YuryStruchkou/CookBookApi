using System;
using System.Linq;
using CookBook.DAL.Data;
using CookBook.Domain.Models;
using CookBook.Presentation.Filters;
using CookBook.Presentation.Helpers;
using CookBook.Presentation.JWT;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CookBook.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.OverrideDefaultModelValidation<ModelValidationAttribute>();
            services.AddCors();
            services.AddJwtAuthentication(Configuration);
            services.AddAuthorizationPolicies();
            services.AddScoped<JwtFactory>(sp =>
                new JwtFactory(Configuration["Tokens:Issuer"], Configuration["Tokens:Key"], Convert.ToInt32(Configuration["Tokens:ValidForMinutes"])));
            services.AddScoped<RefreshTokenFactory>(sp =>
                new RefreshTokenFactory(Convert.ToInt32(Configuration["Tokens:RefreshTokenBytes"]), Convert.ToInt32(Configuration["Tokens:RefreshValidForDays"])));
            services.AddIdentity<ApplicationUser, IdentityRole<int>>(IdentityOptionsHelper.ConfigureIdentityOptions)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.RegisterCustomServices();
            services.AddAutoMapperProfiles();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, RoleManager<IdentityRole<int>> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            roleManager.SeedRoles().Wait();
            app.UseExceptionMiddleware();
            app.UseCors(builder => builder.WithOrigins(Configuration.GetSection("Cors:Origins").Get<string[]>())
                .AllowCredentials().AllowAnyHeader().AllowAnyMethod());
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
