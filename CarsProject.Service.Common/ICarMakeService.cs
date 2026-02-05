using CarsProject.Common;
using CarsProject.Model;

namespace CarsProject.Service.Common
{
    public interface ICarMakeService
    {
        Task<PagedResult<CarMake>> GetCarMakesAsync(PFSParameters pfs);
        Task<CarMake> GetCarMakeByIdAsync(int id);
        Task<CarMake> AddCarMakeAsync(CarMake carMake);
        Task<CarMake> UpdateCarMakeAsync(int id, CarMake carMake);
        Task<bool> DeleteCarMakeAsync(int id);
    }
}