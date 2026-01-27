using CarsProject.Common;
using CarsProject.Model;

namespace CarsProject.Repository.Common
{
    public interface ICarEngineTypeRepository : IGenericRepository<CarEngineType>
    {
        Task<IEnumerable<CarEngineType>> GetAllCarEngineTypesAsync(PSFParameters psf);
    }
}
