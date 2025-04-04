using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CarsProject.Model.Common
{
    public interface ICarEngineType
    
    {
        int Id { get; set; }
        string Type { get; set; }
        string Abrv { get; set; }

    }
}
