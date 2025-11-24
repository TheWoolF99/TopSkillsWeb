using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public class DbContextFactory
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public DbContextFactory(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public ApplicationContext Create(Type repositoryType)
        {
            var services = httpContextAccessor.HttpContext.RequestServices;

            var dbContexts = services.GetService<Dictionary<Type, ApplicationContext>>();

            if (!dbContexts.ContainsKey(repositoryType))
            {
                dbContexts[repositoryType] = services.GetService<ApplicationContext>();
            }

            return dbContexts[repositoryType];
        }
    }
}