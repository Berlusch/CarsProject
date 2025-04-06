namespace CarsProject.Model.Common
{
    public interface ICarRegistration
    {
        int Id { get; set; }
        string RegistrationNumber { get; set; }
        ICarOwner CarOwner { get; set; }  
        ICarModel CarModel { get; set; }  
    }
}

