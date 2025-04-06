using CarsProject.Model.Common;

namespace CarsProject.Model
{
    public class CarMake : EntityBase, ICarMake
    {
        public string Name { get; set; } = "";
        public string Abrv { get; set; } = "";

    }
}
