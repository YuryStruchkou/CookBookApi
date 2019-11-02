using System;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CookBook.BLL.Services;
using CookBook.CoreProject.Constants;
using CookBook.CoreProject.Helpers;
using CookBook.CoreProject.Interfaces;
using CookBook.Domain.ElasticSearch;
using CookBook.Domain.Mappers;
using CookBook.Domain.Models;
using CookBook.Domain.ResultDtos;
using CookBook.Presentation.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Nest;
using Policy = CookBook.CoreProject.Constants.Policy;

namespace CookBook.Presentation.Helpers
{
    public static class ServiceCollectionHelper
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = Convert.ToBoolean(config["Tokens:RequireHttps"]);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["Tokens:Issuer"],
                        ValidAudience = config["Tokens:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Tokens:Key"]))
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = context =>
                        {
                            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                            context.Response.ContentType = "application/json";
                            var result = new ErrorDto(context.Response.StatusCode, "Invalid JWT token.");
                            context.Response.WriteAsync(JsonConvertHelper.SerializeObjectCamelCase(result)).RunSynchronously();
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        public static void AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policy.ApiUser, policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimsIdentity.DefaultRoleClaimType, UserRoleNames.User, UserRoleNames.Admin);
                });
                options.AddPolicy(Policy.ApiAdmin, policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimsIdentity.DefaultRoleClaimType, UserRoleNames.Admin);
                });
            });
        }

        public static void OverrideDefaultModelValidation<T>(this IServiceCollection services) where  T : ActionFilterAttribute
        {
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });
            services.AddScoped<T>();
        }

        public static void AddAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AccountProfile), typeof(RecipeProfile));
        }

        public static void AddElasticSearch(this IServiceCollection services, IConfiguration config)
        {
            var url = config["ElasticSearch:Url"];
            var defaultIndex = config["ElasticSearch:Index"];
            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex);
            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);
        }

        public static void RegisterCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IRecipeService, RecipeService>();
            services.AddScoped<ICookieService, CookieService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISearchService, SearchService>();
        }
    }
}
