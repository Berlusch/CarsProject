using CarsProject.Common;
using CarsProject.Model;

namespace CarsProject.Service.Common
{
    public interface ICarModelService
    {
        Task<PagedResult<CarModel>> GetCarModelsAsync(PFSParameters pfs);
        Task<CarModel> GetCarModelByIdAsync(int id);
        Task<CarModel> AddCarModelAsync(CarModel carModel);
        Task<CarModel> UpdateCarModelAsync(int id, CarModel carModel);
        Task<bool> DeleteCarModelAsync(int id);
    }
}
