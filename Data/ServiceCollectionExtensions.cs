
using Core;
using Core.Account;
using Data.Repository;
using Interfaces.Photo;
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

            #region Синглтоны 
            services.AddSingleton<DbContextFactory>();
            services.AddSingleton<IRepository<User>, UserRepository>();
            services.AddSingleton<IPhotoRepository, PhotoRepository>();
            #endregion

            return services;
        }
    }

}
