namespace CarsProject.Model.Common
    {
        public interface ICarMake:IEntityBase
        {            
            string Name { get; set; }
            string Abrv { get; set; }
        }
    }

