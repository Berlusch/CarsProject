using System.ComponentModel.DataAnnotations;

namespace CarsProject.WebApi.DTO
{
    public record CarOwnerInsertUpdateDto
    (
        [Required(ErrorMessage = "First name is required.")]
        string FirstName,

        [Required(ErrorMessage = "Last name is required.")]
        string LastName,

        [Required(ErrorMessage = "Date of birth is required.")]
        DateOnly DateOfBirth
    );
}
