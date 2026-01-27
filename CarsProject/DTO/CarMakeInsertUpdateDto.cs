using System.ComponentModel.DataAnnotations;

namespace CarsProject.WebApi.DTO
{
    public record CarMakeInsertUpdateDto
    (
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 20 characters.")]
        string Name,
        [Required(ErrorMessage = "Abbreviation is required.")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Abbreviation must be between 2 and 10 characters.")]
        string Abrv
    );
}
