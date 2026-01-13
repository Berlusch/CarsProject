using CarsProject.WebApi;

namespace CarsProject.Repository.Common
{
    public interface ICarEngineTypeRepository : ICRUDRepository<CarEngineType>
    {
        Task<IEnumerable<CarEngineType>> GetAllCarEngineTypesAsync();        
        
    }
}
