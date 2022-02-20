using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi
{
    public class AppDbContext : DbContext
    {
        public DbSet<IfcElement> IfcElements { get; set; } = default!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
    }
}
