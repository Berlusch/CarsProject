namespace CarsProject.WebApi.DTO
{
    public record CarOwnerReadDto
    (
        int Id,
        string FirstName,
        string LastName,
        DateOnly DateOfBirth

    );
    
}
