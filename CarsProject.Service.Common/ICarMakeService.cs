using CarsProject.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarsProject.Service
{
    public interface ICarMakeService
    {
        Task<IEnumerable<CarMake>> GetAllCarMakesAsync();
        Task<CarMake> GetCarMakeByIdAsync(int id);
        Task<CarMake> AddCarMakeAsync(CarMake carMake);
        Task<CarMake> UpdateCarMakeAsync(CarMake carMake);
        Task<bool> DeleteCarMakeAsync(int id);
    }
}

