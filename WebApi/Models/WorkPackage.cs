namespace WebApi.Models
{
    public class WorkPackage
    {
        public int WorkPackageId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
        public List<ModelElement>? ModelElements { get; set; }
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        public int CciEePpId { get; set; }
        public CciEePp? CciEePp { get; set; }
    }
}
