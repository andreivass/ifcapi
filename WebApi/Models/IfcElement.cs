namespace WebApi.Models
{
    public class IfcElement
    {
        public int IfcElementId { get; set; }
        public string? NameEe { get; set; }
        public string? NameEn { get; set; }
        public string? IfcName { get; set; }
        public string? IfcType { get; set; }
        public string? PredefinedType { get; set; }
        public bool SystemCreated { get; set; }
    }
}
