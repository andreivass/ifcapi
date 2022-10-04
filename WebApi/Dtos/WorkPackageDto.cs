using WebApi.Models;

namespace WebApi.Dtos
{
    public class WorkPackageDto
    {
        public int WorkPackageId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
        public List<ModelElementDto>? ModelElements { get; set; }
        public int ProjectId { get; set; }
        public int CciEePpId { get; set; }
        public string? ClassificatorNameEe { get; set; }
    }
}
