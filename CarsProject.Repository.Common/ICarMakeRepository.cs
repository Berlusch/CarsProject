using CarsProject.Model;


namespace CarsProject.Repository.Common
{
    public interface ICarMakeRepository: ICRUDRepository<CarMake>
       
    {
        Task<IEnumerable<CarMake>> GetAllCarMakesAsync();
    }
}
