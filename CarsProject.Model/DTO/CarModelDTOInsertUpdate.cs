using System.ComponentModel.DataAnnotations;

namespace CarsProject.Model.DTO
{
    public record CarModelDTOInsertUpdate
    (
        [Required(ErrorMessage = "Name is required.")]
    string Name,
        [Required(ErrorMessage = "Abbreviation is required.")]
    string Abrv,
        [Required(ErrorMessage = "Car make ID is required.")]
    int CarMakeId

    //CarEngineTypeId is not required because it is not used in the insert/update operation (only lookup is used)

    );
}
