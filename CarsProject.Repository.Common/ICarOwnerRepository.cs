using CarsProject.Model;

namespace CarsProject.Repository.Common
{
    public interface ICarOwnerRepository : ICRUDRepository<CarOwner>

    {
        Task<IEnumerable<CarOwner>> GetAllCarOwnersAsync();
    }
}
