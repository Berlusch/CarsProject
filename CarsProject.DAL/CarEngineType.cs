using System.ComponentModel.DataAnnotations.Schema;

namespace CarsProject.DAL
{
    internal class CarEngineType:EntityBase
    {        
        public string Type { get; set; } = "";
        public string Abrv { get; set; } = "";

    }
}
