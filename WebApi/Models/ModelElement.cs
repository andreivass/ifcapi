namespace WebApi.Models
{
    /// <summary>
    /// Model for IFC model elements.
    /// </summary>
    public class ModelElement
    {
        public int ModelElementId { get; set; }
        public string? Guid { get; set; }
        public int ExpressId { get; set; }
        public string? IfcType { get; set; }
        public string? IfcStorey { get; set; }
        public string? Name { get; set; }
        public string? ObjectType { get; set; }
        public ICollection<ModelElementInWorkPackage>? ModelElementInWorkPackages { get; set; }
    }
}
