using Microsoft.EntityFrameworkCore;

public class DatabaseContext: DbContext
{
    public DbSet<Post> Posts { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<Image> Images { get; set; }

    public DatabaseContext()
    {
        // Database.EnsureDeleted();
        Database.EnsureCreated();

        // var image1 = new Image
        // {
        //     Id = 1,
        //     Code = "password1",
        //     CreationDateTime = new DateTime(2008, 5, 1, 8, 20, 52),
        //     LastModificationDateTime = new DateTime(2008, 5, 1, 8, 20, 52),
        // };
        
        // Users.Add(user1);
        // Images.Add(image1);

        // SaveChanges();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=surf_site;Username=postgres;Password=Sinusoid360");
    }
}