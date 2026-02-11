using Microsoft.EntityFrameworkCore;
using my_economy_api.Models;

namespace my_economy_api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<FixedCost> FixedCosts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FixedCost>()
                .Property(f => f.CreatedAt)
                .HasDefaultValueSql("now()") // <--- Esto crea el DEFAULT en Postgres
                .ValueGeneratedOnAdd();      // <--- Esto le dice a EF: "No envíes nada, deja que la DB lo cree"
        }
    }
}
