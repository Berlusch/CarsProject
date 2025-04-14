using CarsProject.Model;


namespace CarsProject.Repository.Common
{
    public interface ICarModelRepository : ICRUDRepository<CarModel>

    {
        Task<IEnumerable<CarModel>> GetAllCarModelsAsync();
    }
}
