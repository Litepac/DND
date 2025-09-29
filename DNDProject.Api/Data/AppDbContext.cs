using DNDProject.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DNDProject.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Container> Containers => Set<Container>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        // Simple constraints
        b.Entity<Container>()
            .Property(x => x.Type)
            .HasMaxLength(100);

        // Seed data så du kan se noget i Swagger med det samme
        b.Entity<Container>().HasData(
            new Container
            {
                Id = 1,
                Type = "Plast",
                Material = ContainerMaterial.Plast,
                SizeLiters = 2500,
                WeeklyAmountKg = 120,
                LastFillPct = 82,
                PreferredPickupFrequencyDays = 14
            },
            new Container
            {
                Id = 2,
                Type = "Jern",
                Material = ContainerMaterial.Jern,
                SizeLiters = 7000,
                WeeklyAmountKg = 900,
                LastFillPct = 46,
                PreferredPickupFrequencyDays = 21
            }
        );
    }
}
