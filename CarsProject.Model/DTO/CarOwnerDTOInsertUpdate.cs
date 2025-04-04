using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsProject.Model.DTO
{
    public record CarOwnerDTOInsertUpdate
    (
        [Required(ErrorMessage = "First name is required.")]
        string FirstName,

        [Required(ErrorMessage = "Last name is required.")]
        string LastName,

        [Required(ErrorMessage = "Date of birth is required.")]
        DateOnly DateOfBirth
    );
}
