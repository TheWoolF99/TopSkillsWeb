using Core;
using Core.Account;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Data
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
           : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<UserAvatar> UserAvatars { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        //public DbSet<GroupStudent> GroupStudents { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<GroupStudent>()
            //.HasKey(t => new { t.StudentId, t.GroupId });

            //modelBuilder.Entity<GroupStudent>()
            //    .HasOne(sc => sc.Student)
            //    .WithMany(s => s.GroupStudents)
            //    .HasForeignKey(sc => sc.StudentId);

            //modelBuilder.Entity<GroupStudent>()
            //    .HasOne(sc => sc.Group)
            //    .WithMany(c => c.GroupStudents)
            //    .HasForeignKey(sc => sc.GroupId);


            //modelBuilder.Entity<Attendance>()
            //    .HasMany(s => s.groupStudents)
            //    .WithMany(s => s.attendances);
                

            modelBuilder.Entity<Course>().Property(b => b.DateCreate)
            .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Group>().Property(b => b.DateCreate)
            .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Student>().Property(b => b.DateCreate)
            .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Teacher>().Property(b => b.DateCreate)
            .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Attendance>().Property(b => b.DateCreate)
            .HasDefaultValueSql("getdate()");

        }



    }
}
