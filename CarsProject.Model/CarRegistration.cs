using CarsProject.Model.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarsProject.Model
{
    public class CarRegistration : EntityBase, ICarRegistration
    {       
        public string RegistrationNumber { get; set; } = "";

        [ForeignKey("carOwner")]
        public required ICarOwner CarOwner { get; set; }
        [ForeignKey("carModel")]
        public required ICarModel CarModel { get; set; }

        
        
    }
}

