
using Core;
using Core.Account;
using Data.Repository;
using Data.Services;
using Data.WebUser;
using Interfaces;
using Interfaces.Abonement;
using Interfaces.Attendance;
using Interfaces.Course;
using Interfaces.Group;
using Interfaces.Logger;
using Interfaces.Photo;
using Interfaces.Student;
using Interfaces.Teacher;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Z.EntityFramework.Plus;


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

            AuditManager.DefaultConfiguration.AutoSavePreAction = (context, audit) => {

                (context as ApplicationContext).AuditEntries.AddRange(audit.Entries);
            };

            services.AddScoped<Dictionary<Type, ApplicationContext>>();

            #region Синглтоны 
            services.AddScoped<DbContextFactory>();
            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IGroup, GroupRepository>();
            services.AddScoped<ICourse, CourseRepository>();
            services.AddScoped<IStudent, StudentRepository>();
            services.AddScoped<ITeacher, TeacherRepository>();
            services.AddScoped<IAttendance, AttendanceRepository>();
            services.AddScoped<ILoggerRep, LoggerRepository>();
            services.AddScoped<IAbonement, AbonementRepository>();
            services.AddScoped<IGlobalOptions, GlobalOptionsRepository>();
            services.AddScoped<IWebUser, WebUserRepository>();
            services.AddScoped<IAccessesRepository, AccessesRepository>();
            #endregion

            return services;
        }
    }

}
