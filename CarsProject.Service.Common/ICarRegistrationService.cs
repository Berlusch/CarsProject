using CarsProject.Common; 
using CarsProject.Model;

namespace CarsProject.Service.Common
{
    public interface ICarRegistrationService
    {
        Task<PagedResult<CarRegistration>> GetCarRegistrationsAsync(PFSParameters pfs);
        Task<CarRegistration> GetCarRegistrationByIdAsync(int id);
        Task<CarRegistration> AddCarRegistrationAsync(CarRegistration carRegistration);
        Task<CarRegistration> UpdateCarRegistrationAsync(int id, CarRegistration carRegistration);
        Task<bool> DeleteCarRegistrationAsync(int id);
    }
}

