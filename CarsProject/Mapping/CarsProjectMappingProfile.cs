using AutoMapper;
using CarsProject.Model;
using CarsProject.Model.DTO;

namespace CarsProject.Mapping
{
    public class CarsProjectMappingProfile : Profile
    {
        public CarsProjectMappingProfile()
        {
            // CarEngineType Mapping
            CreateMap<CarEngineType, CarEngineTypeDTORead>();

            // CarOwner Mapping
            CreateMap<CarOwner, CarOwnerDTORead>();
            CreateMap<CarOwnerDTOInsertUpdate, CarOwner>();

            // CarMake Mapping
            CreateMap<CarMake, CarMakeDTORead>();
            CreateMap<CarMakeDTOInsertUpdate, CarMake>();

            // CarModel Mapping
            CreateMap<CarModel, CarModelDTORead>()            
            .ForCtorParam("CarMakeName", opt => opt.MapFrom(src => src.CarMake.Name))
            .ForCtorParam("CarEngineTypeType", opt => opt.MapFrom(src => src.CarEngineType.Type));

            CreateMap<CarModel, CarModelDTOInsertUpdate>()
                .ForMember(dest => dest.CarMakeId, opt => opt.MapFrom(src => src.CarMake.Id))
                .ForMember(dest => dest.CarEngineTypeId, opt => opt.MapFrom(src => src.CarEngineType.Id));                
               
            CreateMap<CarModelDTOInsertUpdate, CarModel>();
            CreateMap<CarModel, CarModelDTOInsertUpdate>();

            // CarRegistration Mapping
            CreateMap<CarRegistration, CarRegistrationDTORead>()
                .ForCtorParam("CarModelName", opt => opt.MapFrom(src => src.CarModel.Name))
                .ForCtorParam("CarOwnerFirstNameLastName", opt => opt.MapFrom(src => $"{src.CarOwner.FirstName} {src.CarOwner.LastName}"));

            CreateMap<CarRegistrationDTOInsertUpdate, CarRegistration>()
                .ForMember(dest => dest.CarModel, opt => opt.MapFrom(src => src.CarModelId))
                .ForMember(dest => dest.CarOwner, opt => opt.MapFrom(src => src.CarOwnerId));
        }
    }
}
