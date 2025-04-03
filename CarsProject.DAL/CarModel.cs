using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsProject.DAL
{
    public class CarModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Abrv { get; set; } = "";
        public required CarMake CarMake { get; set; }
        public required CarEngineType CarEngineType { get; set; }
    }
}
