using CarsProject.WebApi.Common;

namespace CarsProject.WebApi
{
    public class CarModel : EntityBase, ICarModel
    {
        
        public string Name { get; set; } = "";
        public string Abrv { get; set; } = ""; 
        public required CarEngineType CarEngineType { get; set; }                
        public required CarMake CarMake { get; set; }

        
        ICarEngineType ICarModel.CarEngineType
        {
            get => CarEngineType;
            set => CarEngineType = (CarEngineType)value;
        }
                
        ICarMake ICarModel.CarMake
        {
            get => CarMake;
            set => CarMake = (CarMake)value;
        }
    }
}
