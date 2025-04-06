using CarsProject.Model;

namespace CarsProject.Repository.Common
{
    public interface ICarEngineTypeRepository
    {
        Task<IEnumerable<CarEngineType>> GetAllAsync();        
        Task<CarEngineType> GetByIdAsync(Guid id);
    }
}
