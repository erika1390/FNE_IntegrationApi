using Integration.Infrastructure.Interfaces.Parametric;

namespace Integration.Infrastructure.Interfaces.UnitOfWork
{
    public interface IParametricDbUOW : IDisposable
    {
        ICityRepository CityRepository { get; set; }
        IDepartmentRepository DepartmentRepository { get; set; }
        IIdentificationDocumentTypeRepository IdentificationDocumentTypeRepository { get; set; }
        Task<int> SaveChangesAsync();
    }
}