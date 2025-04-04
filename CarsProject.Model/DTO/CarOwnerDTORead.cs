using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsProject.Model.DTO
{
    public record CarOwnerDTORead
    (
        int Id,
        string FirstName,
        string LastName,
        DateOnly DateOfBirth

    );
}
