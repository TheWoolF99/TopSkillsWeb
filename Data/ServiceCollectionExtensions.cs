
using Core;
using Data.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;


namespace Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEfRepositories(this IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<ApplicationContext>(
                options =>
                {
                    options.UseSqlServer(connectionString);
                }
                , ServiceLifetime.Transient
                );





            services.AddScoped<Dictionary<Type, ApplicationContext>>();

            services.AddSingleton<DbContextFactory>();

            #region Синглтоны 
            services.AddTransient<IRepository<User>, UserRepository>();
            #endregion


            return services;
        }
    }

}
