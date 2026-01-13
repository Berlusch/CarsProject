using CarsProject.WebApi.Common;

namespace CarsProject.WebApi
{
    public class CarEngineType : EntityBase, ICarEngineType
    {
        
        public string Type { get; set; } = "";
        public string Abrv { get; set; } = "";

    }
}
