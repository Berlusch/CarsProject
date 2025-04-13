namespace CarsProject.DAL
{
    internal class CarModel : EntityBase
    {             
        public string Name { get; set; } = "";
        public string Abrv { get; set; } = "";
        public required CarMake CarMake { get; set; }
        public required CarEngineType CarEngineType { get; set; }
    }
}
