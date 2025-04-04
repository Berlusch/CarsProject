using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsProject.Model.DTO
{
    public record CarEngineTypeDTORead
   (
        int Id,
        string Type,
        string Abrv
    );
}
