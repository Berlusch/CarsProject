namespace CarsProject.DAL
{
    internal class CarOwner:EntityBase
    {        
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateOnly DateOfBirth { get; set; }               
        
    }
}
