namespace WebApi.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? IfcFileName { get; set; }
        public List<WorkPackage> WorkPackages{ get; set; }
    }
}
