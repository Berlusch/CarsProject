namespace CarsProject.Model.Common
{
    public interface ICarModel:IEntityBase
    {        
        string Name { get; set; }

        int CarEngineTypeId { get; set; }
        ICarEngineType CarEngineType { get; set; }
    }
}
