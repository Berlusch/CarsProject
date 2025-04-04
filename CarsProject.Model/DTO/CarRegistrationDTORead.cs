using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
