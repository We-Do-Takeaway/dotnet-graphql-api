using Microsoft.EntityFrameworkCore;

namespace WeDoTakeawayAPI.GraphQL.Model
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Many-to-many: Basket <-> Item
            modelBuilder
                .Entity<BasketItem>()
                .HasKey(bi => new {bi.ItemId, bi.BasketId});

            // Many-to-many: Item <-> Ingredient
            modelBuilder
                .Entity<ItemIngredient>()
                .HasKey(ii => new {ii.ItemId, ii.IngredientId});
            
            // Many-to-many: Section <-> Item
            modelBuilder
                .Entity<SectionItem>()
                .HasKey(si => new {si.SectionId, si.ItemId});
        }

        public DbSet<Basket> Baskets { get; set; } = default!;

        public DbSet<Ingredient> Ingredients { get; set; } = default!;
        
        public DbSet<Item> Items { get; set; } = default!;

        public DbSet<Menu> Menus { get; set; } = default!;

        public DbSet<Section> Sections { get; set; } = default!;
    }
}