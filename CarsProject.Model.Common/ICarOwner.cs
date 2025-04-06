namespace CarsProject.Model.Common
{
    public interface ICarOwner 
    {
        int Id { get; set; }
        string FirstName { get; set; } 
        string LastName { get; set; }
        DateOnly DateOfBirth { get; set; }

    }
}
