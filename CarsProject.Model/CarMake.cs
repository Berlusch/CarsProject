using CarsProject.WebApi.Common;

namespace CarsProject.WebApi
{
    
    public class CarMake : EntityBase, ICarMake
    {
        public string Name { get; set; } = "";
        public string Abrv { get; set; } = "";

    }
}
