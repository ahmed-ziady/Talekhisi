using Microsoft.EntityFrameworkCore;
using Talekhisi.Entities;
namespace Talekhisi.Data
{
    public class AppDbContext(DbContextOptions <AppDbContext> options): DbContext(options)
    {
        public DbSet <User> Users{ get; set; }

    }
}
