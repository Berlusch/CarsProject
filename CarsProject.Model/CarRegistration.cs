using CarsProject.Model.Common;

namespace CarsProject.Model
{
    public class CarRegistration : EntityBase, ICarRegistration
    {
        public string RegistrationNumber { get; set; } = "";

        public int CarOwnerId { get; set; }
        public int CarModelId { get; set; }
        
        public CarOwner CarOwner { get; set; } = null!;
        public CarModel CarModel { get; set; } = null!;

        
        ICarOwner ICarRegistration.CarOwner
        {
            get => CarOwner;
            set => CarOwner = (CarOwner)value;
        }

        ICarModel ICarRegistration.CarModel
        {
            get => CarModel;
            set => CarModel = (CarModel)value;
        }
    }
}


