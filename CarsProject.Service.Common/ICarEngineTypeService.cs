using CarsProject.Model; 
using CarsProject.Common; 

namespace CarsProject.Service.Common
{
    public interface ICarEngineTypeService
    {
        Task<IEnumerable<CarEngineType>> GetCarEngineTypesAsync(PFSParameters pfs);
        Task<CarEngineType> GetCarEngineTypeByIdAsync(int id);
    }
}