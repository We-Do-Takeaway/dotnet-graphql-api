using Microsoft.EntityFrameworkCore;

namespace WeDoTakeawayAPI.GraphQL.Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Basket> Baskets { get; set; } = default!;

        public DbSet<BasketItem> BasketItems { get; set; } = default!;
    }
}