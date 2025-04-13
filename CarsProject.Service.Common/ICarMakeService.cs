using CarsProject.Model;

namespace CarsProject.Service
{
    public interface ICarMakeService
    {
        Task<IEnumerable<CarMake>> GetCarMakesPagedAsync(int pageNumber, int pageSize, string sortBy, string filter); // Dodana PFS metoda
        Task<CarMake> GetCarMakeByIdAsync(int id);  
        Task<CarMake> AddCarMakeAsync(CarMake carMake);  
        Task<CarMake> UpdateCarMakeAsync(int id, CarMake carMake);  
        Task<bool> DeleteCarMakeAsync(int id);
    }
}

