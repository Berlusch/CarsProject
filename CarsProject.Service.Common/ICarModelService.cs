using CarsProject.WebApi.DTO;

namespace CarsProject.Service
{
    public interface ICarModelService
    {
        Task<IEnumerable<CarModelDTORead>> GetCarModelsPagedAsync(int pageNumber, int pageSize, string sortBy, string filter);
        Task<CarModelDTORead> GetCarModelByIdAsync(int id);
        Task<CarModelDTORead> AddCarModelAsync(CarModelDTOInsertUpdate carModelDto);
        Task<CarModelDTORead> UpdateCarModelAsync(int id, CarModelDTOInsertUpdate carModelDto);
        Task<bool> DeleteCarModelAsync(int id);
    }
}
