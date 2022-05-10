using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Hair.Repository.Abstract;
using Hair.Repository.Concrete;
using Microsoft.EntityFrameworkCore;
using Hair.Repository.Context;

namespace Hair.Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<ApplicationDbContext>(options => {
                var connectionString = configuration.GetConnectionString("MySqlConnection");

                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                //options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                var db = serviceProvider.GetRequiredService<ApplicationDbContext>();
            }
            
            return services;
        }
    }
}