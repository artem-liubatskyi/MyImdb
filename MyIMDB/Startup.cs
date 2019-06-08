using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyIMDB.Data;
using MyIMDB.DataAccess;
using MyIMDB.Interfaces;
using MyIMDB.Services;
using MyIMDB.Services.Hashing;
using MyIMDB.Web.Helpers;

namespace MyIMDB
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddTransient<IHasher, Hasher>();
            services.AddTransient<IMovieService, MovieService>();
            services.AddTransient<IMoviePersonService, MoviePersonService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IRateRepository, RateRepository>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            #region Authentication
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IAccountService>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = userService.Get(userId);
                        if (user == null)
                        {
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            #endregion
            
            services.AddDbContext<ImdbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ImdbContext).GetTypeInfo().Assembly.GetName().Name)));


            services.AddAutoMapper();
            services.AddMvc();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
