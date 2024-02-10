using Core;
using Core.Account;
using Core.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Drawing;
using Z.EntityFramework.Plus;


namespace Data
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public ApplicationContext(DbContextOptions<ApplicationContext> options, IHttpContextAccessor httpContextAccessor)
           : base(options)
        {
            Database.EnsureCreated();
            this.httpContextAccessor = httpContextAccessor;

            AuditManager.DefaultConfiguration.AutoSavePreAction = (context, audit) =>
            {
                var cont = (context as ApplicationContext);
                cont.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(cont.ChangeTracker.DebugView.LongView);
                Console.ResetColor();
                cont.AuditEntries.AddRange(audit.Entries);
            };
        }
    

        public DbSet<UserAvatar> UserAvatars { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<LoggerItem> Logger { get; set; }
        //Логирование
        public DbSet<AuditEntry> AuditEntries { get; set; }
        public DbSet<AuditEntryProperty> AuditEntryProperties { get; set; }
        //public DbSet<GroupStudent> GroupStudents { get; set; }



        public override int SaveChanges()
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

            var audit = new Audit();
            if ((bool)httpContextAccessor.HttpContext?.User?.Identity.IsAuthenticated)
            {
                audit.CreatedBy = httpContextAccessor.HttpContext?.User?.Identity?.Name;
            }
            audit.PreSaveChanges(this);
            
            var rowAffecteds = base.SaveChanges();
            audit.PostSaveChanges();

            if (audit.Configuration.AutoSavePreAction != null)
            {
                audit.Configuration.AutoSavePreAction(this, audit);
                base.SaveChanges();
            }

            return rowAffecteds;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

            var audit = new Audit();
            if ((bool)httpContextAccessor.HttpContext?.User?.Identity.IsAuthenticated)
            {
                audit.CreatedBy = httpContextAccessor.HttpContext?.User?.Identity?.Name;
            }
            audit.PreSaveChanges(this);
            

            var rowAffecteds = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            audit.PostSaveChanges();

            if (audit.Configuration.AutoSavePreAction != null)
            {
                audit.Configuration.AutoSavePreAction(this, audit);
                await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }

            return rowAffecteds;
        }


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
