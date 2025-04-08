using CarsProject.Model.DTO;

namespace CarsProject.Service
{

    public interface ICarMakeService
    {
        Task<IEnumerable<CarMakeDTORead>> GetAllCarMakesAsync();
        Task<CarMakeDTORead> GetCarMakeByIdAsync(int id);
        Task<CarMakeDTORead> AddCarMakeAsync(CarMakeDTOInsertUpdate carMakeDto);
        Task<CarMakeDTORead> UpdateCarMakeAsync(int id, CarMakeDTOInsertUpdate carMakeDto);
        Task<bool> DeleteCarMakeAsync(int id);
    }
}