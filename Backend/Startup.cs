using Backend.Models;
using Backend.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal;
using Pomelo.EntityFrameworkCore.MySql.Internal;

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
            var asd = Configuration.GetConnectionString("MariaDBConnectionString");
            services.AddDbContext<MariaDBContext>(options => options.UseMySql(Configuration.GetConnectionString("MariaDBConnectionString"),
                ServerVersion.AutoDetect(Configuration.GetConnectionString("MariaDBConnectionString")

                )));

            services.AddControllers();
            services.AddIdentity<User,UserRole>().AddEntityFrameworkStores<MariaDBContext>().AddDefaultTokenProviders();
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

            app.UseEndpoints(endpoints =>
            {
                
                endpoints.MapControllers();
            });
        }
    }
}
