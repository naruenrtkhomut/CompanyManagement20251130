using Microsoft.EntityFrameworkCore;

namespace api.Models.Database.PSQL
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> inOptions) : base(inOptions) { }

        public DbSet<Data.Employee> employee { get; set; }
    }
}
