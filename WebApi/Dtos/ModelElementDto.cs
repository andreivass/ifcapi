using WebApi.Models;

namespace WebApi.Dtos
{
    public class ModelElementDto
    {
        public string? Guid { get; set; }
        public int ExpressId { get; set; }
        public string? IfcType { get; set; }
        public string? IfcStorey { get; set; }
        public string? Name { get; set; }
        public string? ObjectType { get; set; }
    }
}
