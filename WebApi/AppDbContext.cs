using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Models.Identity;

namespace WebApi
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<CciEePp>? CciEePps { get; set; }
        public DbSet<ModelElementInWorkPackage>? ModelElementInWorkPackages { get; set; }
        public DbSet<ModelElement>? ModelElements { get; set; }
        public DbSet<Project>? Projects { get; set; }
        public DbSet<WorkPackage>? WorkPackages { get; set; }
        public virtual DbSet<AppUser>? AppUsers { get; set; }
        public virtual DbSet<AppRole>? AppRoles { get; set; }
    }
}
