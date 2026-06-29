using Microsoft.EntityFrameworkCore;

public class DatabaseContext: DbContext
{
    public DbSet<Post> Posts { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<ImagePostShip> ImagePostShips { get; set; }

    public DatabaseContext()
    {
        // Database.EnsureDeleted();
        Database.EnsureCreated();

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=surf_site;Username=postgres;Password=Sinusoid360");
    }
}