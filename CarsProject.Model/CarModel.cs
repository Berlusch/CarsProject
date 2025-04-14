using CarsProject.Model.Common;

namespace CarsProject.Model
{
    public class CarModel : EntityBase, ICarModel
    {
        
        public string Name { get; set; } = "";
        public string Abrv { get; set; } = ""; 
        public required CarEngineType CarEngineType { get; set; }                
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
