using CarsProject.Model.Common;

namespace CarsProject.Model
{
    public class CarModel : ICarModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";

        public int CarEngineTypeId { get; set; }
        public required CarEngineType CarEngineType { get; set; }

        // Dodavanje CarMake kao vanjski ključ
        public int CarMakeId { get; set; }
        public required CarMake CarMake { get; set; }

        // Implementacija ICarModel interfejsnog svojstva CarEngineType
        ICarEngineType ICarModel.CarEngineType
        {
            get => CarEngineType;
            set => CarEngineType = (CarEngineType)value;
        }

        // Implementacija ICarModel interfejsnog svojstva CarMake
        ICarMake ICarModel.CarMake
        {
            get => CarMake;
            set => CarMake = (CarMake)value;
        }
    }
}
