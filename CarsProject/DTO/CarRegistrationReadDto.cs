namespace CarsProject.WebApi.DTO
{
    public record CarRegistrationReadDto
    (
        int Id,
        string RegistrationNumber,
        string CarOwnerFirstNameLastName,
        string CarModelName
        
        );
}
