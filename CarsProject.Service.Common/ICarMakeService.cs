using CarsProject.WebApi.DTO;

namespace CarsProject.Service
{
    public interface ICarMakeService
    {
        Task<IEnumerable<CarMakeDTORead>> GetCarMakesPagedAsync(int pageNumber, int pageSize, string sortBy, string filter);
        Task<CarMakeDTORead> GetCarMakeByIdAsync(int id);
        Task<CarMakeDTORead> AddCarMakeAsync(CarMakeDTOInsertUpdate carMakeDto);
        Task<CarMakeDTORead> UpdateCarMakeAsync(int id, CarMakeDTOInsertUpdate carMakeDto);
        Task<bool> DeleteCarMakeAsync(int id);
    }
}
