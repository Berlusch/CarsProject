using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsProject.Model.DTO
{
    public record CarMakeDTORead
    (
        int Id,
        string Name,
        string Abrv
    );
}
