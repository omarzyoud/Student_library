using Microsoft.EntityFrameworkCore;
using SCR.API.Models.Domain;

namespace SCR.API.Data
{
    public class SCRDbContext : DbContext
    {
        public SCRDbContext(DbContextOptions<SCRDbContext> options) : base(options)
        {
            //test githup       
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<SubCat> SubCats { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Notification> Notifications { get; set; } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Bookmark>().HasKey(b => new { b.StdId, b.MaterialId });
            modelBuilder.Entity<Rate>().HasKey(r => new { r.StdId, r.MaterialId });
            modelBuilder.Entity<Comment>().HasKey(c => new { c.StdId, c.MaterialId });
            modelBuilder.Entity<SubCat>().HasKey(sc => new { sc.SubjectId, sc.CatId });
            modelBuilder.Entity<Report>().HasKey(report => new { report.StdId, report.MaterialId });
            modelBuilder.Entity<Notification>().HasKey(n => n.NotificationId);



            modelBuilder.Entity<Bookmark>()
        .HasOne(b => b.Student)
        .WithMany(s => s.Bookmarks)
        .HasForeignKey(b => b.StdId)
        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Student)
                .WithMany(s => s.Comments)
                .HasForeignKey(c => c.StdId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Student)
                .WithMany(s => s.Reports)
                .HasForeignKey(r => r.StdId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Rate>()
                .HasOne(r => r.Student)
                .WithMany(s => s.Rates)
                .HasForeignKey(r => r.StdId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Bookmark>()
                .HasOne(b => b.Material)
                .WithMany(m => m.Bookmarks)
                .HasForeignKey(b => b.MaterialId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Student)
                .WithMany()  // Removed the navigation property from Student
                .HasForeignKey(n => n.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Material)
                .WithMany()  // Removed the navigation property from Material
                .HasForeignKey(n => n.MaterialId)
                .OnDelete(DeleteBehavior.NoAction);

            // Additional configurations...



        }
    }
}
