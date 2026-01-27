using CarsProject.WebApi;

namespace CarsProject.Repository.Common
{
    public interface ICarOwnerRepository : IGenericRepository<CarOwner>

    {
        Task<IEnumerable<CarOwner>> GetAllCarOwnersAsync();
    }
}
