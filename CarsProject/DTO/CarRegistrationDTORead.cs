namespace CarsProject.WebApi.DTO
{
    public record CarRegistrationDTORead
    (
        int Id,
        string RegistrationNumber,
        string CarOwnerFirstNameLastName,
        string CarModelName
        
        );
}
