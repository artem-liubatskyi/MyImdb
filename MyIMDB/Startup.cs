using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyIMDB.Data;
using MyIMDB.Data.Entities;
using MyIMDB.Services;
using MyIMDB.Services.Configuration;
using MyIMDB.Services.Helpers;
using MyIMDB.Services.MapperProfiles;
using MyIMDB.Web.Helpers;
using System;
using System.Text;
using System.Threading.Tasks;
using TmdbClient;
using TmdbClient.Mapping;

namespace MyIMDB
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureAuthentication(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.Secret));

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => 
                policy.RequireClaim("Role", "ApiAccess"));
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ITmdbClient, TmdbClient.TmdbClient>();
            services.AddTransient<ITmdbService, TmdbService>();
            services.RegisterServiceDependencies();


            services.AddDbContext<ImdbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            ConfigureAuthentication(services);

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ServicesMapperProfile());
                mc.AddProfile(new TmdbServiceMappingProfile());
            });

            services.AddSingleton(mappingConfig.CreateMapper());

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ITmdbService serv)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            serv.Seed().Wait();
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials());

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
