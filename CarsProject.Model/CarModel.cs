using CarsProject.Model;
using CarsProject.Model.Common;

namespace CarsProject.Model
{
    public class CarModel : ICarModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";

        public int CarEngineTypeId { get; set; }

        public required CarEngineType CarEngineType { get; set; }

        ICarEngineType ICarModel.CarEngineType
        {
            get => CarEngineType;
            set => CarEngineType = (CarEngineType)value;
        }
    }
}

