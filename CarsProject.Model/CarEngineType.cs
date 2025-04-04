using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsProject.Model.Common;

namespace CarsProject.Model
{
    public class CarEngineType : ICarEngineType
    {
        public int Id { get; set; }
        public string Type { get; set; } = "";
        public string Abrv { get; set; } = "";

    }
}
