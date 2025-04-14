using CarsProject.Model.DTO;

namespace CarsProject.Service
{
    public interface ICarOwnerService
    {
        Task<IEnumerable<CarOwnerDTORead>> GetCarOwnersPagedAsync(int pageNumber, int pageSize, string sortBy, string filter);
        Task<CarOwnerDTORead> GetCarOwnerByIdAsync(int id);
        Task<CarOwnerDTORead> AddCarOwnerAsync(CarOwnerDTOInsertUpdate carOwnerDto);
        Task<CarOwnerDTORead> UpdateCarOwnerAsync(int id, CarOwnerDTOInsertUpdate carOwnerDto);
        Task<bool> DeleteCarOwnerAsync(int id);
    }
}

