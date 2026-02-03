using CarsProject.Common; 
using CarsProject.Model;

namespace CarsProject.Service.Common
{
    public interface ICarOwnerService
    {
        Task<IEnumerable<CarOwner>> GetCarOwnersAsync(PFSParameters pfs);
        Task<CarOwner> GetCarOwnerByIdAsync(int id);
        Task<CarOwner> AddCarOwnerAsync(CarOwner carOwner);
        Task<CarOwner> UpdateCarOwnerAsync(int id, CarOwner carOwner);
        Task<bool> DeleteCarOwnerAsync(int id);
    }
}


