using CarsProject.WebApi.DTO;

namespace CarsProject.Service
{
    public interface ICarRegistrationService
    {
        Task<IEnumerable<CarRegistrationDTORead>> GetCarRegistrationsPagedAsync(int pageNumber, int pageSize, string sortBy, string filter);
        Task<CarRegistrationDTORead> GetCarRegistrationByIdAsync(int id);
        Task<CarRegistrationDTORead> AddCarRegistrationAsync(CarRegistrationDTOInsertUpdate carRegistrationDto);
        Task<CarRegistrationDTORead> UpdateCarRegistrationAsync(int id, CarRegistrationDTOInsertUpdate carRegistrationDto);
        Task<bool> DeleteCarRegistrationAsync(int id);
    }
}

