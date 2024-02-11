
using Core;
using Core.Account;
using Data.Repository;
using Data.Services;
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
            services.AddScoped<Dictionary<Type, ApplicationContext>>();

            #region Синглтоны 
            services.AddSingleton<DbContextFactory>();
            services.AddSingleton<IRepository<User>, UserRepository>();
            services.AddSingleton<IPhotoRepository, PhotoRepository>();
            services.AddSingleton<IGroup, GroupRepository>();
            services.AddSingleton<ICourse, CourseRepository>();
            services.AddSingleton<IStudent, StudentRepository>();
            services.AddSingleton<ITeacher, TeacherRepository>();
            services.AddSingleton<IAttendance, AttendanceRepository>();
            services.AddSingleton<ILoggerRep, LoggerRepository>();
            services.AddSingleton<IAbonement, AbonementRepository>();
            #endregion

            return services;
        }
    }

}
