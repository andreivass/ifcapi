using WebApi.Dtos;
using WebApi.Models;

namespace WebApi.Helpers
{
    /// <summary>
    /// Helper class for dto mapping.
    /// </summary>
    public static class DtoMappingHelper
    {
        /// <summary>
        /// Maps model elements to dtos.
        /// </summary>
        /// <param name="modelXrefs">Model element xrefs.</param>
        /// <returns>Model element dtos.</returns>
        public static List<ModelElementDto> MapModelElementsToDtos(ICollection<ModelElementInWorkPackage> modelXrefs)
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
