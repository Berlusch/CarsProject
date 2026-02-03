using CarsProject.Common;
using CarsProject.Model;

namespace CarsProject.Repository.Common
{
    public interface ICarModelRepository : IGenericRepository<CarModel>
    {
        Task<PagedResult<CarModel>> GetPagedAsync(PFSParameters? parameters = null);
    }
}