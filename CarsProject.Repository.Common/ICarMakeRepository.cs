using CarsProject.WebApi;


namespace CarsProject.Repository.Common
{
    public interface ICarMakeRepository: IGenericRepository<CarMake>
       
    {
        Task<IEnumerable<CarMake>> GetAllCarMakesAsync();
    }
}
