namespace WebApi.Models
{
    public class ModelElement
    {
        public int ModelElementId { get; set; }
        public string? Guid { get; set; }
        public string? ExpressId { get; set; }
        public string? IfcType { get; set; }
        public string? IfcStorey { get; set; }
        public string? Name { get; set; }
        public string? ObjectType { get; set; }
        public int WorkPackageId { get; set; }
        public WorkPackage? WorkPackage { get; set; }
        public int IfcElementId { get; set; }
        public IfcElement? IfcElement { get; set; }
    }
}
