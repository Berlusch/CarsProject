using CarsProject.Model; 
using CarsProject.Common; 

namespace CarsProject.Service.Common
{
    public interface ICarEngineTypeService
    {
        Task<IEnumerable<CarEngineType>> GetCarEngineTypesAsync(PSFParameters pfs);
        Task<CarEngineType> GetCarEngineTypeByIdAsync(int id);
    }
}