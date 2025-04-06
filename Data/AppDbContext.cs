using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Member> Members { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Member>().HasKey(m => m.Id);
        modelBuilder.Entity<InventoryItem>().HasKey(i => i.Id);
        modelBuilder.Entity<Booking>().HasKey(b => b.Id);
        modelBuilder.Entity<Member>().HasMany(m => m.Bookings).WithOne(b => b.Member);
        modelBuilder.Entity<InventoryItem>().HasMany(i => i.Bookings).WithOne(b => b.InventoryItem);
    }
}