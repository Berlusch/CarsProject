namespace CarsProject.Model.Common
{
    public interface ICarModel:IEntityBase
    {        
        string Name { get; set; }   
        string Abrv { get; set; }
        ICarEngineType CarEngineType { get; set; }        
        ICarMake CarMake { get; set; }
    }
}
