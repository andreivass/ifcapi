using WebApi.Dtos;
using WebApi.Models;

namespace WebApi.Helpers
{
    public static class DtoMappingHelper
    {
        public static List<ModelElementDto> MapModelElementsToDtos(List<ModelElementInWorkPackage> modelXrefs)
        {
            var dtos = new List<ModelElementDto>();
            foreach (var xref in modelXrefs)
            {
                if (xref.ModelElement != null)
                {
                    dtos.Add(new()
                    {
                        Guid = xref.ModelElement.Guid,
                        Name = xref.ModelElement.Name,
                        ObjectType = xref.ModelElement.ObjectType,
                        ExpressId = xref.ModelElement.ExpressId,
                        IfcStorey = xref.ModelElement.IfcStorey,
                        IfcType = xref.ModelElement.IfcType
                    });
                }
            }

            return dtos;
        }
    }
}
