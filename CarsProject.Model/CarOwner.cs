using CarsProject.WebApi.Common;

namespace CarsProject.WebApi
{
    public class CarOwner : EntityBase, ICarOwner
    {        
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateOnly DateOfBirth { get; set; }

    }
}
