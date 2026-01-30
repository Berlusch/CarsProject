using CarsProject.Model.Common;

namespace CarsProject.Model
{
    public class CarModel : EntityBase, ICarModel
    {
        public string Name { get; set; } = "";
        public string Abrv { get; set; } = "";
        
        public int CarMakeId { get; set; }
        public int CarEngineTypeId { get; set; }
        
        public virtual required CarMake CarMake { get; set; } 
        public virtual required CarEngineType CarEngineType { get; set; } 

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

