using Core.Entities;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Country> Countries { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Warehouse>()
        .HasOne(w => w.City)
        .WithMany(c => c.Warehouses)
        .HasForeignKey(w => w.CityId)
        .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Warehouse>()
            .HasOne(w => w.Country)
            .WithMany()
            .HasForeignKey(w => w.CountryId)
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Warehouse>()
               .HasIndex(w => w.Name)
               .IsUnique();

        modelBuilder.Entity<Item>()
               .HasIndex(i => i.Name)
               .IsUnique();


        modelBuilder.Entity<Item>()
               .Property(i => i.CostPrice)
               .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<Item>()
            .Property(i => i.MSRPPrice)
            .HasColumnType("decimal(18, 2)");




    }
}
