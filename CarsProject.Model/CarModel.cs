using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsProject.Model.Common;

namespace CarsProject.Model
{
    public class CarModel: ICarModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Abrv { get; set; } = "";
        public ICarMake CarMake { get; set; }
        public ICarEngineType CarEngineType { get; set; }

        // Constructor to initialize the properties
        public CarModel(ICarMake carMake, ICarEngineType carEngineType)
        {
            CarMake = carMake ?? throw new ArgumentNullException(nameof(carMake));
            CarEngineType = carEngineType ?? throw new ArgumentNullException(nameof(carEngineType));
        }
    }
}
