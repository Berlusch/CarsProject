namespace CarsProject.Model.Common
{
    public interface ICarOwner: IEntityBase 
    {
        string FirstName { get; set; } 
        string LastName { get; set; }
        DateOnly DateOfBirth { get; set; }

    }
}
