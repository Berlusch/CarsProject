namespace CarsProject.Model.DTO
{
    public record CarRegistrationDTORead
    (
        int Id,
        string RegistrationNumber,
        string CarOwnerFirstNameLastName,
        string CarModelName
        
        );
}
