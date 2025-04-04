using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    int CarMakeId,
        [Required(ErrorMessage = "Car engine type ID is required.")]
    int CarEngineTypeId

    );
}
