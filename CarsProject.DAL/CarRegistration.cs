namespace CarsProject.DAL
{
    internal class CarRegistration : EntityBase
    {        
        public string RegistrationNumber { get; set; } = "";
        public required CarOwner CarOwner { get; set; } 
        public required CarModel CarModel { get; set; }

    }
}
