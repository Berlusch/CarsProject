using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsProject.Model.Common;

namespace CarsProject.Model
{
    public class CarMake : ICarMake
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Abrv { get; set; } = "";

    }
}
