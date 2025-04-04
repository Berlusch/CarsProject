using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsProject.Model.Common;

namespace CarsProject.Model
{
    public class CarRegistration : ICarRegistration
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; } = "";
        public ICarOwner CarOwner { get; set; }
        public ICarModel CarModel { get; set; }

        //Constructor to initialize the non-nullable properties
        public CarRegistration(ICarOwner carOwner, ICarModel carModel)
        {
            CarOwner = carOwner ?? throw new ArgumentNullException(nameof(carOwner));
            CarModel = carModel ?? throw new ArgumentNullException(nameof(carModel));
        }

    }
}
