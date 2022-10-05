namespace WebApi.Models
{
    /// <summary>
    /// Xref model between ModelElement and WorkPackage.
    /// </summary>
    public class ModelElementInWorkPackage
    {
        public int ModelElementInWorkPackageId { get; set; }

        public int ModelElementId { get; set; }
        public ModelElement? ModelElement { get; set; }

        public int WorkPackageId { get; set; }
        public WorkPackage? WorkPackage { get; set; }
    }
}
