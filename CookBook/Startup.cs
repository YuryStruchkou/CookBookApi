using AutoMapper;
using CookBook.DAL.Data;
using CookBook.Domain.Models;
using CookBook.Presentation.Filters;
using CookBook.Presentation.Helpers;
using CookBook.Presentation.JWT;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.OverrideDefaultModelValidation<ModelValidationAttribute>();
            services.AddCors();
            services.AddJwtAuthentication(Configuration);
            services.AddAuthorizationPolicies();
            services.AddScoped<JwtFactory>(sp =>
                new JwtFactory(Configuration["Tokens:Issuer"], Configuration["Tokens:Key"]));
            services.AddIdentity<ApplicationUser, IdentityRole<int>>(IdentityOptionsHelper.ConfigureIdentityOptions)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddAutoMapperProfiles();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionMiddleware();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
