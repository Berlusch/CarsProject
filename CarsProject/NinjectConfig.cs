using Ninject;
using CarsProject.Repository;
using CarsProject.Repository.Common;

namespace CarsProject
{
    public static class NinjectConfig
    {
        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();

            // Repository bindings
            kernel.Bind<ICarMakeRepository>().To<CarMakeRepository>();
            kernel.Bind<ICarModelRepository>().To<CarModelRepository>();
            kernel.Bind<ICarOwnerRepository>().To<CarOwnerRepository>();
            kernel.Bind<ICarRegistrationRepository>().To<CarRegistrationRepository>();
            kernel.Bind<ICarEngineTypeRepository>().To<CarEngineTypeRepository>();

            return kernel;
        }
    }
}
