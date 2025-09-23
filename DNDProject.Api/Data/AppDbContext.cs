using DNDProject.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DNDProject.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Container> Containers => Set<Container>();
}
