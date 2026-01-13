using CarsProject.WebApi;


namespace CarsProject.Repository.Common
{
    public interface ICarMakeRepository: ICRUDRepository<CarMake>
       
    {
        Task<IEnumerable<CarMake>> GetAllCarMakesAsync();
    }
}
