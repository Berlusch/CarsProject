using CarsProject.Common;
using CarsProject.WebApi;

namespace CarsProject.Repository.Common
{
    public interface ICarEngineTypeRepository : IGenericRepository<CarEngineType>
    {
        Task<IEnumerable<CarEngineType>> GetAllCarEngineTypesAsync(PSFParameters psf);
    }
}
