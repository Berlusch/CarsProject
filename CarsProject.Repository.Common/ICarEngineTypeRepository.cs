using CarsProject.Model;

namespace CarsProject.Repository.Common
{
    public interface ICarEngineTypeRepository : ICRUDRepository<CarEngineType>
    {
        Task<IEnumerable<CarEngineType>> GetAllCarEngineTypesAsync();        
        
    }
}
