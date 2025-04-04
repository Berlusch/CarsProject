using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace CarsProject.Model.Common
    {
        public interface ICarMake
        {
            int Id { get; set; }
            string Name { get; set; }
            string Abrv { get; set; }
        }
    }

