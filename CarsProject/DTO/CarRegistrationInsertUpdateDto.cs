using System.ComponentModel.DataAnnotations;

namespace CarsProject.WebApi.DTO
{
    public record CarRegistrationInsertUpdateDto
    (
        [Required(ErrorMessage = "Registration number is required.")]   
        string RegistrationNumber,
        [Required(ErrorMessage = "Car owner ID is required.")]
        int CarOwnerId,
        [Required(ErrorMessage = "Car model ID is required.")]
        int CarModelId 

        );
}
