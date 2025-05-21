using Integration.Shared.DTO.Parametric;

namespace Integration.Application.Interfaces.Parametric
{
    public interface IIdentificationDocumentTypeService
    {
        Task<IEnumerable<IdentificationDocumentTypeDTO>> GetAllActiveAsync();
    }
}