using Microsoft.EntityFrameworkCore;
using Talekhisi.Entities;
namespace Talekhisi.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<LectureNote> LectureNotes { get; set; }
        public DbSet<Subject> Subjects { get; set; }

      

        protected override void OnModelCreating( ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           modelBuilder.Entity<LectureNote>()
                .HasOne(l => l.Subject)
                .WithMany(s => s.LectureNotes)
                .HasForeignKey(l => l.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);





            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<LectureNote>().ToTable("LectureNotes");
            modelBuilder.Entity<Subject>().ToTable("Subjects");
            // Add any additional configuration here
        }
    }

}
