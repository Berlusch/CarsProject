namespace CarsProject.Model.DTO
{
    public record CarOwnerDTORead
    (
        int Id,
        string FirstName,
        string LastName,
        DateOnly DateOfBirth

    );
    
}
