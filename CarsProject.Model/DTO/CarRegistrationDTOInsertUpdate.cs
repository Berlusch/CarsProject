using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CarsProject.Model.DTO
{
    public record CarRegistrationDTOInsertUpdate
    (
        [Required(ErrorMessage = "Registration number is required.")]   
        string RegistrationNumber,
        [Required(ErrorMessage = "Car owner ID is required.")]
        int CarOwnerId,
        [Required(ErrorMessage = "Car model ID is required.")]
        int CarModelId 

        );
}
