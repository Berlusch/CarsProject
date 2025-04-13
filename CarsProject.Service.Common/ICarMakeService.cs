using CarsProject.Model;
using CarsProject.Model.DTO;

namespace CarsProject.Service
{
    public interface ICarMakeService
    {
        Task<IEnumerable<CarMake>> GetCarMakesPagedAsync(int pageNumber, int pageSize, string sortBy, string filter); // Dodana PFS metoda
        Task<CarMake> GetCarMakeByIdAsync(int id);  
        Task<CarMake> AddCarMakeAsync(CarMake carMake);
        Task<CarMakeDTORead> UpdateCarMakeAsync(int id, CarMakeDTOInsertUpdate carMakeDto);
        Task<bool> DeleteCarMakeAsync(int id);
    }
}

