namespace WebApi.Models
{
    /// <summary>
    /// Model representing work package.
    /// </summary>
    public class WorkPackage
    {
        public int WorkPackageId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
        public ICollection<ModelElementInWorkPackage>? ModelElementInWorkPackages { get; set; }
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        public int CciEePpId { get; set; }
        public CciEePp? CciEePp { get; set; }
    }
}
