using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsProject.Model.Common
{
    public interface ICarRegistration 
    {
        int Id { get; set; }
        string RegistrationNumber { get; set; }
        ICarOwner CarOwner { get; set; }
        ICarModel CarModel { get; set; }

    }
}
