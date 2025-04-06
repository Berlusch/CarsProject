using CarsProject.Model.Common;
using System.ComponentModel.DataAnnotations.Schema;


namespace CarsProject.Model
{
    public class CarModel : EntityBase, ICarModel
    {        
        public string Name { get; set; } = "";
        public string Abrv { get; set; } = "";

        [ForeignKey("carMake")]
        public required ICarMake CarMake { get; set; }

        [ForeignKey("carEngineType")]
        public required ICarEngineType CarEngineType { get; set; }
                
    }
}
