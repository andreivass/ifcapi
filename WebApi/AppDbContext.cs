using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi
{
    public class AppDbContext : DbContext
    {
        public DbSet<CciEePp> CciEePps { get; set; } = default!;
        public DbSet<IfcElement> IfcElements { get; set; } = default!;
        public DbSet<ModelElement> ModelElements { get; set; } = default!;
        public DbSet<Project> Projects { get; set; } = default!;
        public DbSet<WorkPackage> WorkPackages { get; set; } = default!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
    }
}
