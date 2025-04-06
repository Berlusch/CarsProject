using CarsProject.Model.Common;

namespace CarsProject.Model
{
    public class CarEngineType : EntityBase, ICarEngineType
    {
        
        public string Type { get; set; } = "";
        public string Abrv { get; set; } = "";

    }
}
