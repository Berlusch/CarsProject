using CarsProject.Repository;
using CarsProject.Repository.Common;
using CarsProject.Service;
using Ninject;

namespace CarsProject.WebApi
{
    public static class DependencyInjection
    {
        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            
            kernel.Bind<ICarRegistrationService>().To<CarRegistrationService>();
            kernel.Bind<ICarModelService>().To<CarModelService>();
            kernel.Bind<ICarEngineTypeService>().To<CarEngineTypeService>();
            kernel.Bind<ICarOwnerService>().To<CarOwnerService>();
            kernel.Bind<ICarMakeService>().To<CarMakeService>();
            kernel.Bind<ICarMakeRepository>().To<CarMakeRepository>();
            kernel.Bind<ICarModelRepository>().To<CarModelRepository>();
            kernel.Bind<ICarOwnerRepository>().To<CarOwnerRepository>();
            kernel.Bind<ICarRegistrationRepository>().To<CarRegistrationRepository>();
            kernel.Bind<ICarEngineTypeRepository>().To<CarEngineTypeRepository>();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>();

            return kernel;
        }
    }
}
