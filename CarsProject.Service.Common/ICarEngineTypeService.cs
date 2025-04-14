using CarsProject.Model.DTO;

namespace CarsProject.Service
{
    public interface ICarEngineTypeService
    {
        Task<IEnumerable<CarEngineTypeDTORead>> GetCarEngineTypesPagedAsync(int pageNumber, int pageSize, string sortBy, string filter);
        Task<CarEngineTypeDTORead> GetCarEngineTypeByIdAsync(int id);
        
    }
}
