namespace CarsProject.Model.Common
{
    public interface ICarRegistration : IEntityBase
    {
        string RegistrationNumber { get; set; }

        int CarOwnerId { get; set; }
        int CarModelId { get; set; }

        ICarOwner CarOwner { get; set; }
        ICarModel CarModel { get; set; }
    }
}



