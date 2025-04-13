using CarsProject.Model.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsProject.Model
{
    
    public class CarMake : EntityBase, ICarMake
    {
        public string Name { get; set; } = "";
        public string Abrv { get; set; } = "";

    }
}
