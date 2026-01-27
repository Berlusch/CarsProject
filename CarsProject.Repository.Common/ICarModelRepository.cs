using CarsProject.WebApi;


namespace CarsProject.Repository.Common
{
    public interface ICarModelRepository : IGenericRepository<CarModel>

    {
        Task<IEnumerable<CarModel>> GetAllCarModelsAsync();
    }
}
