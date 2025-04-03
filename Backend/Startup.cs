using Backend.Models;
using Backend.Persistence;
using Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal;
using Pomelo.EntityFrameworkCore.MySql.Internal;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Text.Unicode;

namespace Backend
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

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder
                        .SetIsOriginAllowed(origin => true)
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        );
            });

            var asd = Configuration.GetConnectionString("MariaDBConnectionString");
            services.AddDbContext<MariaDBContext>(options => options.UseMySql(Configuration.GetConnectionString("MariaDBConnectionString"),
                ServerVersion.AutoDetect(Configuration.GetConnectionString("MariaDBConnectionString")

                )));

            services.AddIdentity<User, UserRole>().AddEntityFrameworkStores<MariaDBContext>().AddDefaultTokenProviders();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    //TODO key to .env
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))

                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        // Log the error

                        Debug.WriteLine($"Authentication failed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        Debug.WriteLine($"Authentication failed: {"challenged"}");

                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync("{\"error\":\"Unauthorized\"}");
                    },
                    OnForbidden = context =>
                    {
                        Debug.WriteLine($"Authentication failed: {"challenged"}");

                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync("{\"error\":\"Unauthorized\"}");
                    },
                    OnTokenValidated =  async context => {
                        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
                        var claimsP = context.Principal;

                        var userId = claimsP?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                        if (userId == null)
                        {
                            context.Fail("Invalid token");
                            return;
                        }
                        var user = await userManager.FindByIdAsync(userId!);
                        if  (user == null)
                        {
                            context.Fail("User not found");
                            return;
                        }

                        context.Success();
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser() 
                    .Build();
                options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
            });

            services.AddControllers();

            services.AddScoped<UserService>();
            services.AddScoped<CompetitionService>();
            services.AddScoped<SubmissionService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowAllOrigins");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllers();
            });
        }
    }
}
