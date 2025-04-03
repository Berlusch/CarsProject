using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsProject.DAL
{
    public class CarRegistration
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; } = "";
        public required CarOwner CarOwner { get; set; } 
        public required CarModel CarModel { get; set; }

    }
}
