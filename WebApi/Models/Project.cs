using WebApi.Models.Identity;

namespace WebApi.Models
{
    /// <summary>
    /// Model representing IFC project.
    /// </summary>
    public class Project
    {
        public int ProjectId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? IfcFileName { get; set; }
        public ICollection<WorkPackage>? WorkPackages { get; set; }

        public AppUser? AppUser { get; set; }
        public string? AppUserId { get; set; }
    }
}
